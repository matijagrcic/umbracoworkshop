/// <reference path="../../models/ifailedserviceresponse.ts" />
/// <reference path="../app.ts" />

/**
 * Provides an interface that defines the methods & properties of the base service
 */
interface IBaseService {

    PluginKey: string;

    GetJsonAsync<T>(url: string, cache?: boolean, specificSuccessFunction?: Function, specificErrorFunction?: Function): T;

    PostJsonAsync<T>(url: string, data: T, specificSuccessFunction?: Function, specificErrorFunction?: Function): T;

    HandleSuccessServiceResponse<T>(response: ng.IHttpPromiseCallbackArg<T>): void;

    HandleFailedServiceResponse(response: IFailedServiceResponse);
}

/**
 * Provides the base angular service class definition
 */
class BaseService implements IBaseService {

    /**
    * A boolean that indicates whether the api caching should be implemented
    */
    static ApiCaching: boolean = false;

    /**
    * A boolean that indicates whether the api caching should be implemented
    */
    static EmptyFunction = () => { };

    /**
     * Creates a base service
     */
    constructor(private dependencyContext: DependencyModule.IDependencyContext) {

        // default plugin key
        this.PluginKey = "UmbracoService";
    }

    /**
     * Provides a key for the name of the plugin consuming the service
     */
    PluginKey: string;

    /**
     * Returns json object of specific type
     *  @param url - Url of the resource
     *  @param cache - Cache the response, default is true
     *  @param specificSuccessFunction - A call back function that can be executed on the success of the request
     *  @param specificErrorFunction - A call back function that can be executed on the failure of the request
     */
    GetJsonAsync<T>(url: string, cache: boolean = true, specificSuccessFunction: Function = () => { }, specificErrorFunction: Function = () => { }): ng.IHttpPromiseCallbackArg<T> {
        return this.dependencyContext.Http.get(url, { cache: cache }).then(
            (response: ng.IHttpPromiseCallbackArg<T>) => {

                var data: T = response.data;

                specificSuccessFunction(data);

                this.HandleSuccessServiceResponse(response);

                return data;
            },
            (response: ng.IHttpPromiseCallbackArg<IFailedServiceResponse>) => {

                specificErrorFunction();

                this.HandleFailedServiceResponse(response.data);

            });
    }

    /**
     * Posts json object to the desired resource asynchronously
     *  @param url - Url of the resource for POST
     *  @param payload - Data that will be sent on POST
     *  @param specificSuccessFunction - A call back function that can be executed on the success of the request
     *  @param specificErrorFunction - A call back function that can be executed on the failure of the request
     */
    PostJsonAsync<T, TU>(url: string, payload: TU, specificSuccessFunction: Function = BaseService.EmptyFunction, specificErrorFunction: Function = BaseService.EmptyFunction): ng.IHttpPromiseCallbackArg<T> {
        return this.dependencyContext.Http.post(url, payload).then(
            (response: ng.IHttpPromiseCallbackArg<T>) => {

                var data: T = response.data;

                specificSuccessFunction(data);

                this.HandleSuccessServiceResponse(response);

                return data;
            },
            (response: ng.IHttpPromiseCallbackArg<IFailedServiceResponse>) => {
                this.HandleFailedServiceResponse(response.data);
                specificErrorFunction();
            });
    }

    /**
      * Handle a success service response
      * @param {baseService: IBaseService} Provides the base service that the call was made on
      * @param {response: IFailedServiceResponse} Provides the object representing the exception information returned from the service call
    */
    HandleSuccessServiceResponse<T>(response: ng.IHttpPromiseCallbackArg<T>) {

        // log the response to console
        this.dependencyContext.ConsoleLogger.log(response);
        this.dependencyContext.ConsoleLogger.log(response.data);
    }

    /**
    * Handle a failed service response
    * @param {baseService: IBaseService} Provides the base service that the call was made on
    * @param {response: IFailedServiceResponse} Provides the object representing the exception information returned from the service call
    */
    HandleFailedServiceResponse(response: IFailedServiceResponse) {

        // log the response to console
        this.dependencyContext.ConsoleLogger.log(response);

        // instantiate a new object for the response
        var serviceResponse = new FailedServiceResponse(this.PluginKey, response);

        // call the notification service passing the error info
        if (UmbracoApp.Common.IfUndefined(serviceResponse.ExceptionMessage)) {
            this.dependencyContext.NotificationsService.error("Error", "An unknown error has occured.");
        } else {
            this.dependencyContext.NotificationsService.error(serviceResponse.Message, serviceResponse.ToString());
        }
    }
}
