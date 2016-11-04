/**
 * Provides an interface for a failed response from any request to retrieve data from an api controller
 */
interface IFailedServiceResponse {
    Message: string;
    ExceptionMessage: string;
    ExceptionType: string;
    StackTrace: string;

    /**
    * Implements a ToString method on the object to format the properties
    */
    ToString();
}

/**
 * Provides an implementation of a failed service response object
 */
class FailedServiceResponse implements IFailedServiceResponse {

    Message: string;
    ExceptionMessage: string;
    ExceptionType: string;
    StackTrace: string;

    /**
     * Creates an instance of the faield service response object
     * @constructor
     * @param {plugin: string} The name of the plugin instantaing this object
     * @param {responseObject: IFailedServiceResponse} The response object from the request to retrieve the service
     */
    constructor(plugin: string, responseObject: IFailedServiceResponse) {
        // set the internal properties of the object
        this.Message = UmbracoApp.Common.Format("{0} In {1}.", responseObject.Message, plugin);
        this.ExceptionMessage = responseObject.ExceptionMessage;
        this.ExceptionType = responseObject.ExceptionType;
        this.StackTrace = responseObject.StackTrace;
    }

    /**
    * Implements a toString method on the object
    */
    ToString = function () {
        return UmbracoApp.Common.Format("{0} {1}", this.ExceptionMessage, this.ExceptionType);
    }
}
