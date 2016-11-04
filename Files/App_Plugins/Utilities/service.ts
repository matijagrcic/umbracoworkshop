/// <reference path="../../Scripts/Core/app.ts" />
/// <reference path="../../Scripts/Core/DependencyContext.ts" />
/// <reference path="../../Scripts/Core/Base/service.ts" />
/// <reference path="../../Scripts/Services/UrlService.ts" />

/**
 * Utilities module definition
 */
module UtilitiesModule {

    /**
     * Provides an interface that defines the methods & properties of the utilities service
     */
    export interface IUtilitiesService extends IBaseService {
        urlService: UrlModule.IUrlService;
        $routeParams: any;
    }

    /**
    * Provides a class for the utilities service
    */
    export class UtilitiesService extends BaseService implements IUtilitiesService, IBaseService {

        // inject dependencies
        static $Inject = ["dependencyContext", "urlService", "$routeParams"];

        /**
         * Creates a utilities service
         * @constructor
         * @param {dependencyContext: IDependencyContext} Provides a dependency context object for injecting dependencies
         * @param {urlService: IUrlService} Provides a service for retrieving the service api urls
         */
        constructor(dependencyContext: DependencyModule.IDependencyContext,
            public urlService: UrlModule.IUrlService, public $routeParams: any) {

            super(dependencyContext);

            this.PluginKey = "UtilitiesService";
        }
    }
}

// set up the service in the angular app
app.service("utilitiesService", UtilitiesModule.UtilitiesService);
