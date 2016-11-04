/// <reference path="../app.ts" />
/// <reference path="service.ts" />

/**
 * Provides the base angular scope interface definition
 */
interface IBaseController {

    /**
    * The base service
   */
    BaseService: IBaseService;

    /**
     * The dependency context
    */
    DependencyContext: DependencyModule.IDependencyContext;

    /**
    * Gets the "active" node id to use for any api request
    */
    GetActiveNodeId(): number;

    /**
    * Gets the "selected" node id to use for any api request e.g. if the user right clicks on a menu item while another item is active
    */
    GetSelectedNodeId(): number;

    /**
    * Sync the tree by node path so that the AppState currentNode is selected.
    * @param path Path of the node.
    * @param specificNodeId Id of the node you want to navigate to.
    * @param navigateToSelectedNode True by default, will change the url and navigate to the current node.
    * @param forceReload False by default, secifies whether to force reload the node data from the server even if it already exists in the tree currently
    * @param activate True by default, optional, specifies whether to set the synced node to be the active node
    */
    SyncTreeByPath(path: string, specificNodeId?: number, navigateToSelectedNode?: boolean, forceReload?: boolean, activate?: boolean): void;

    /**
    * Navigates to the node by its id.
    * @param id Id of the node.
    */
    NavigateByNodeId(id?: number): void;

    /**
    * Navigates to the node by its path.
    * @param id Id of the node.
    */
    NavigateByNodePath(path: string): void;

    /**
    * Gets the current selected node
    */
    GetCurrentNode();

    /**
    * Initilize Umbraco model.value to your type
    * @param model The scope.model
    * @param type The type of your model that will drive the UI
    */
    InitilizeUmbracoModelValue<T>(model: any, type: T): T;

    /**
   * Select Specific Nav Tab
   */
    SelectSpecificNavTab(tabPosition: number): void;

    /**
  * Toggle Navigation
  */
    ToggleNavigation(): void;
}


/**
 * Provides the base angular controller definition
 */
class BaseController implements IBaseController {

    /**
     * Creates a base controller
     * @constructor
     * @param {dependencyContext: IDependencyContext} Provides a dependency context object for injecting dependencies
    */
    constructor(dependencyContext: DependencyModule.IDependencyContext) {

        // set the properties
        this.DependencyContext = dependencyContext;
        this.BaseService = new BaseService(dependencyContext);
    }

    /**
     * The dependency context
    */
    DependencyContext: DependencyModule.IDependencyContext;

    /**
     * The base service
     */
    BaseService: IBaseService;

    InitilizeUmbracoModelValue<T>(model: any, type: T): T {
        //if umbraco value is empty initilize it to a desired type
        if (model.value == '') {
            model.value = type;
        }
        //set umbraco value to your model
        return model.value;
    }

    /**
    * Gets the "active" node id to use for any api request
    * Note that this retrieves the parent node id if the current node is in an unpublished state
    * Note also that in Umbraco 7 the right click custom menu may be brought up without changing the editorState to the node that we right clicked on.
    * So the editorState still gives the active node id not the right clicked node is
    */
    GetActiveNodeId(): number {

        // check that we have an active node
        if (UmbracoApp.Common.HasValue(this.DependencyContext.EditorState.current) === false) {
            return 0;
        }

        // get the parent id of the current node - we get parent cos if we create a new module then the current node will be unpublished and this "id" will be 0
        return this.DependencyContext.EditorState.current.id > 0 ?
            this.DependencyContext.EditorState.current.id :
            this.DependencyContext.EditorState.current.parentId;
    }

    /**
    * Gets the "selected" node id to use for any api request e.g. if the user right clicks on a menu item while another item is active
    * Note that this retrieves the parent node id if the current node is in an unpublished state
    * Note also that in Umbraco 7 the editorState does not always give the "selected" menu item - the appState does provide this
    */
    GetSelectedNodeId(): number {

        // get the menu state
        var currentNode = this.GetCurrentNode();

        // get the parent id of the current node - we get parent cos if we create a new module then the current node will be unpublished and this "id" will be 0
        return currentNode.id > 0 ? currentNode.id : currentNode.parentId;
    }

    /**
    * Sync the tree by node path so that the AppState currentNode is selected.
    * @param path Path of the node.
    * @param specificNodeId Id of the node you want to navigate to.
    * @param navigateToSelectedNode True by default, will change the url and navigate to the current node.
    * @param forceReload False by default, secifies whether to force reload the node data from the server even if it already exists in the tree currently
    * @param activate True by default, optional, specifies whether to set the synced node to be the active node
    */
    SyncTreeByPath(path: string, specificNodeId?: number, navigateToSelectedNode: boolean = true, forceReload: boolean = false, activate: boolean = true) {
        var base = this;
        this.DependencyContext.NavigationService.syncTree({ tree: 'content', path: path, forceReload: forceReload, activate: activate })
            .then(() => {
                if (navigateToSelectedNode === true) {
                    base.NavigateByNodeId();
                } else if (UmbracoApp.Common.HasValue<number>(specificNodeId) === true) {
                    base.NavigateByNodeId(specificNodeId);
                }
            });
    }

    /**
    * Reloaded the nodes by ids
    * @param nodeIds Ids of nodes
    */
    ReloadTreeNodesByIds(nodeIds: number[]) {
        this.DependencyContext.EntityResource.getByIds<any[]>(nodeIds, "Document")
            .then((documents) => {
                _.each(<any>documents, (document: { path: string }) => {
                    this.DependencyContext.NavigationService.syncTree({ tree: "content", path: document.path, forceReload: false, activate: false })
                        .then((syncArgs) => {
                            this.DependencyContext.NavigationService.reloadNode(syncArgs.node);
                        });
                });
            });
    }

    /**
    * Navigates to the node by its id.
    * @param id Id of the node.
    */
    NavigateByNodeId(id?: number) {
        var path = "/content/content/edit/";
        var nodeId = UmbracoApp.Common.HasValue(id) ? id : this.GetSelectedNodeId();

        var pathToNavigate = path + nodeId;
        this.DependencyContext.Location.path(pathToNavigate);
    }

    /**
    * Gets the current selected node
    * Properties include:
    * alias
    * childNodesUrl:
    * cssClass
    * cssClasses
    * hasChildren
    * icon
    * iconFilePath
    * iconIsClass
    * id
    * key
    * level
    * menuUrl
    * metaData
    * name
    * nodeType
    * parent
    * parentId
    * path
    * routePath
    * section
    * style
    * trashed: false
    */
    GetCurrentNode() {
        return this.DependencyContext.AppState.getMenuState("currentNode");
    }

    /**
    * Select Specific Nav Tab
    */
    SelectSpecificNavTab(tabPosition: number) {
        var navTabs = $(".umb-nav-tabs > li:not(.hide)"),
            currentActiveNavTab = $(".umb-nav-tabs > li.active");

        currentActiveNavTab.removeClass("active");
        $(navTabs[tabPosition]).addClass("active");
    }

    // this is needed if you don't want to provide data-target="#tab2" data-toggle="tab" for buttons
    activateTabPanes(tabPosition: number) {
        var tabPanes = $('div.tab-content').find(".tab-pane"),
            currentActiveTabPane = $('div.tab-content').find(".tab-pane.active")

        currentActiveTabPane.removeClass("active");
        $(tabPanes[tabPosition]).addClass("active");
    }

    /**
    * Toggle Navigation
    */
    ToggleNavigation() {

        var isNavigationShown = this.DependencyContext.AppState.getGlobalState("showNavigation");
        if (isNavigationShown) {
            this.DependencyContext.AppState.setGlobalState("showNavigation", false);
            $("#contentwrapper").css("left", "80px");
        } else {
            this.DependencyContext.AppState.setGlobalState("showNavigation", true);
            $("#contentwrapper").css("left", "440px");
        }
    }

    /**
    * Navigates to node for specific path
    */
    NavigateByNodePath(path: string): void {
        this.SyncTreeByPath(path, null, true, false, false);
    }   
}
