/**
 * Dependency module definition
 */
module DependencyModule {

    /**
     * Provides an interface that defines the methods & properties of the dependency context
     */
    export interface IDependencyContext {

        /**
         * The root scope
         */
        Scope: IBaseScope;

        /**
         * Provides the core angular http service
         */
        Http: ng.IHttpService;

        /**
         * Provides a console logger object
         */
        ConsoleLogger: ng.ILogService;

        /**
        * The umbraco notification service
        */
        NotificationsService: umb.services.INotificationsService;

        /**
        * The umbraco navigation service
        */
        NavigationService: umb.services.INavigationService;

        /**
       * The angular location service
       */
        Location: ng.ILocationService;

        /**
        * The current umbraco editor state
        */
        EditorState: umb.services.IEditorState;

        /**
        * The current umbraco app state
        * Provides access to state that can changed as the user interacts with the back office such as menu state
        */
        AppState: umb.services.IAppState;

        /**
       * The umbraco dialog service
       */
        DialogService: umb.services.IDialogService;

         /**
       * The umbraco content resource
       */
        ContentResource: umb.resources.IContentResource;

         /**
       * The umbraco tree service
       */
        TreeService: umb.services.ITreeService;

          /**
       * The umbraco entity resource
       */
        EntityResource: umb.resources.IEntityResource;
    }

    /**
    * Provides a class for the dependency context
    */
    export class DependencyContext implements IDependencyContext {

        // inject dependencies
        static $Inject = ["$rootScope", "$http", "$log", "notificationsService", "navigationService", "$location", "editorState", "appState", "dialogService", "contentResource", "treeService", "entityResource"];

        /**
         * Creates a dependency context
         * @constructor
         * @param {$scope: any} The angular scope - provides the angular scope for the custom data type
         * @param {$http: IHttpService} Provides the core angular http context
         * @param {$q: IQService} Provides the core angular context that can be used to run functions asynchronously
         * @param {$log: any} The injected object for logging - provides a means to write out to the console
         * @param {notificationsService: any} The notification service - used to write out messages in the UI
         * @param {navigationService: any} The navigation service from the umbraco client api
         * @param {$location: any} The angular location service
         * @param {editorState: any} The editor state used to provide access to the properties of the current node on the client side
         * @param {appState: any} The app state used to provide access to state that can changed as the user interacts with the back office such as menu state
         * @param {dialogService: any} The dialog service from the umbraco client api
         * @param {treeService: ITreeService} Allow access to Tree service
         */
        constructor(
            $rootScope: any,
            $http: ng.IHttpService,
            $log: any,
            notificationsService: umb.services.INotificationsService,
            navigationService: umb.services.INavigationService,
            $location: ng.ILocationService,
            editorState: umb.services.IEditorState,
            appState: umb.services.IAppState,
            dialogService: umb.services.IDialogService,
            contentResource: umb.resources.IContentResource,
            treeService: umb.services.ITreeService,
            entityResource: umb.resources.IEntityResource) {

            // set the properties
            this.Scope = $rootScope;
            this.Http = $http;
            this.ConsoleLogger = $log;
            this.NotificationsService = notificationsService;
            this.NavigationService = navigationService;
            this.Location = $location;
            this.EditorState = editorState;
            this.AppState = appState;
            this.DialogService = dialogService;
            this.ContentResource = contentResource;
            this.TreeService = treeService;
            this.EntityResource = entityResource;
        }

        /**
         * The root scope
         */
        Scope: IBaseScope;

        /**
         * Provides the core angular http service
         */
        Http: ng.IHttpService;

        /**
         * Provides a console logger object
         */
        ConsoleLogger: ng.ILogService;

        /**
        * The umbraco notification service
        */
        NotificationsService: umb.services.INotificationsService;

        /**
        * The umbraco navigation service
        */
        NavigationService: umb.services.INavigationService;

        /**
       * The angular location service
       */
        Location: ng.ILocationService;

        /**
        * The current umbraco editor state
        */
        EditorState: umb.services.IEditorState;

        /**
        * The current umbraco app state
        * Provides access to state that can changed as the user interacts with the back office such as menu state
        */
        AppState: umb.services.IAppState;

        /**
        * The umbraco dialog service
        */
        DialogService: umb.services.IDialogService;

        /**
        * The umbraco content resource
        */
        ContentResource: umb.resources.IContentResource;

        /**
         * The umbraco tree resource
         */
        TreeService: umb.services.ITreeService;

        /**
        * The umbraco entity resource
        */
        EntityResource: umb.resources.IEntityResource;
    }
}
app.service("dependencyContext", DependencyModule.DependencyContext);
