var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var DependencyModule;
(function (DependencyModule) {
    var DependencyContext = (function () {
        function DependencyContext($rootScope, $http, $log, notificationsService, navigationService, $location, editorState, appState, dialogService, contentResource, treeService, entityResource) {
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
        DependencyContext.$Inject = ["$rootScope", "$http", "$log", "notificationsService", "navigationService", "$location", "editorState", "appState", "dialogService", "contentResource", "treeService", "entityResource"];
        return DependencyContext;
    }());
    DependencyModule.DependencyContext = DependencyContext;
})(DependencyModule || (DependencyModule = {}));
app.service("dependencyContext", DependencyModule.DependencyContext);
app = angular.module("umbraco");
var UmbracoModule;
(function (UmbracoModule) {
    var Common = (function () {
        function Common() {
            this.DefaultUIDateFormat = "MM/DD/YYYY";
            this.DefaultApiFormat = "YYYY-MM-DD";
            this.Format = function (format) {
                var replacements = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    replacements[_i - 1] = arguments[_i];
                }
                return format.replace(/{(\d+)}/g, function (match, number) { return typeof replacements[number] != 'undefined'
                    ? replacements[number]
                    : match; }).toString();
            };
        }
        Common.prototype.IfUndefined = function (value) {
            var isUndefined = (typeof value === 'undefined');
            return isUndefined;
        };
        Common.prototype.HasValue = function (value) {
            return value !== null && !this.IfUndefined(value);
        };
        Common.prototype.GetValueOrDefault = function (value, defaultVal) {
            if (defaultVal === void 0) { defaultVal = ""; }
            return this.HasValue(value) ? value.toString() : defaultVal;
        };
        Common.prototype.DeleteValueFromArrayByIndex = function (array, index) {
            array.splice(index, 1);
        };
        Common.prototype.IsStringNullOrUndefined = function (value) {
            return (!value || value.length === 0);
        };
        Common.prototype.GetPropertyNameByValue = function (object, propertyValue) {
            for (var property in object) {
                if (object.hasOwnProperty(property) && object[property] == propertyValue) {
                    return property;
                }
            }
        };
        Common.prototype.DeleteObjectFromArrayBySpecificPropertyValue = function (array, propertyValue) {
            var _this = this;
            array.forEach(function (current, index) {
                var propertyName = _this.GetPropertyNameByValue(current, propertyValue);
                if (_this.IfUndefined(propertyName) === false && current[propertyName] == propertyValue) {
                    _this.DeleteValueFromArrayByIndex(array, index);
                }
            });
        };
        Common.prototype.GetObjectFromArrayBySpecificPropertyValue = function (array, propertyValue) {
            var _this = this;
            var foundObject;
            array.forEach(function (current, index) {
                var propertyName = _this.GetPropertyNameByValue(current, propertyValue);
                if (_this.IfUndefined(propertyName) === false && current[propertyName] == propertyValue) {
                    foundObject = current;
                    return;
                }
            });
            return foundObject;
        };
        Common.prototype.PushValueToArray = function (array, value) {
            array.push(value);
        };
        Common.prototype.ReplaceValueInArrayByIndex = function (array, index, value) {
            array[index] = value;
        };
        Common.prototype.GetNumberFromString = function (value) {
            value = value.replace(/\D/g, '');
            return parseInt(value);
        };
        Common.prototype.OpenInNewTab = function (url) {
            var win = window.open(url, '_blank');
            win.focus();
        };
        Common.prototype.FormatApiDate = function (date) {
            var formattedDate = moment(date).format(UmbracoApp.Common.DefaultApiFormat);
            return formattedDate;
        };
        Common.prototype.FormatMoney = function (value, isAlreadyFormatted) {
            if (isAlreadyFormatted === void 0) { isAlreadyFormatted = false; }
            var amountWithCurrency;
            if (isAlreadyFormatted) {
                if (value) {
                    amountWithCurrency = UmbracoApp.Common.Format("${0}", value);
                }
                else {
                    amountWithCurrency = UmbracoApp.Common.Format("${0}", 0.00);
                }
            }
            else {
                var amount = (value / 100);
                amountWithCurrency = accounting.formatMoney(amount);
            }
            return amountWithCurrency;
        };
        return Common;
    }());
    UmbracoModule.Common = Common;
    var UmbracoMain = (function () {
        function UmbracoMain() {
            this.Common = new UmbracoModule.Common();
        }
        return UmbracoMain;
    }());
    UmbracoModule.UmbracoMain = UmbracoMain;
})(UmbracoModule || (UmbracoModule = {}));
var UmbracoApp = new UmbracoModule.UmbracoMain();
var FailedServiceResponse = (function () {
    function FailedServiceResponse(plugin, responseObject) {
        this.ToString = function () {
            return UmbracoApp.Common.Format("{0} {1}", this.ExceptionMessage, this.ExceptionType);
        };
        this.Message = UmbracoApp.Common.Format("{0} In {1}.", responseObject.Message, plugin);
        this.ExceptionMessage = responseObject.ExceptionMessage;
        this.ExceptionType = responseObject.ExceptionType;
        this.StackTrace = responseObject.StackTrace;
    }
    return FailedServiceResponse;
}());
var BaseService = (function () {
    function BaseService(dependencyContext) {
        this.dependencyContext = dependencyContext;
        this.PluginKey = "UmbracoService";
    }
    BaseService.prototype.GetJsonAsync = function (url, cache, specificSuccessFunction, specificErrorFunction) {
        var _this = this;
        if (cache === void 0) { cache = true; }
        if (specificSuccessFunction === void 0) { specificSuccessFunction = function () { }; }
        if (specificErrorFunction === void 0) { specificErrorFunction = function () { }; }
        return this.dependencyContext.Http.get(url, { cache: cache }).then(function (response) {
            var data = response.data;
            specificSuccessFunction(data);
            _this.HandleSuccessServiceResponse(response);
            return data;
        }, function (response) {
            specificErrorFunction();
            _this.HandleFailedServiceResponse(response.data);
        });
    };
    BaseService.prototype.PostJsonAsync = function (url, payload, specificSuccessFunction, specificErrorFunction) {
        var _this = this;
        if (specificSuccessFunction === void 0) { specificSuccessFunction = BaseService.EmptyFunction; }
        if (specificErrorFunction === void 0) { specificErrorFunction = BaseService.EmptyFunction; }
        return this.dependencyContext.Http.post(url, payload).then(function (response) {
            var data = response.data;
            specificSuccessFunction(data);
            _this.HandleSuccessServiceResponse(response);
            return data;
        }, function (response) {
            _this.HandleFailedServiceResponse(response.data);
            specificErrorFunction();
        });
    };
    BaseService.prototype.HandleSuccessServiceResponse = function (response) {
        this.dependencyContext.ConsoleLogger.log(response);
        this.dependencyContext.ConsoleLogger.log(response.data);
    };
    BaseService.prototype.HandleFailedServiceResponse = function (response) {
        this.dependencyContext.ConsoleLogger.log(response);
        var serviceResponse = new FailedServiceResponse(this.PluginKey, response);
        if (UmbracoApp.Common.IfUndefined(serviceResponse.ExceptionMessage)) {
            this.dependencyContext.NotificationsService.error("Error", "An unknown error has occured.");
        }
        else {
            this.dependencyContext.NotificationsService.error(serviceResponse.Message, serviceResponse.ToString());
        }
    };
    BaseService.ApiCaching = false;
    BaseService.EmptyFunction = function () { };
    return BaseService;
}());
var BaseController = (function () {
    function BaseController(dependencyContext) {
        this.DependencyContext = dependencyContext;
        this.BaseService = new BaseService(dependencyContext);
    }
    BaseController.prototype.InitilizeUmbracoModelValue = function (model, type) {
        if (model.value == '') {
            model.value = type;
        }
        return model.value;
    };
    BaseController.prototype.GetActiveNodeId = function () {
        if (UmbracoApp.Common.HasValue(this.DependencyContext.EditorState.current) === false) {
            return 0;
        }
        return this.DependencyContext.EditorState.current.id > 0 ?
            this.DependencyContext.EditorState.current.id :
            this.DependencyContext.EditorState.current.parentId;
    };
    BaseController.prototype.GetSelectedNodeId = function () {
        var currentNode = this.GetCurrentNode();
        return currentNode.id > 0 ? currentNode.id : currentNode.parentId;
    };
    BaseController.prototype.SyncTreeByPath = function (path, specificNodeId, navigateToSelectedNode, forceReload, activate) {
        if (navigateToSelectedNode === void 0) { navigateToSelectedNode = true; }
        if (forceReload === void 0) { forceReload = false; }
        if (activate === void 0) { activate = true; }
        var base = this;
        this.DependencyContext.NavigationService.syncTree({ tree: 'content', path: path, forceReload: forceReload, activate: activate })
            .then(function () {
            if (navigateToSelectedNode === true) {
                base.NavigateByNodeId();
            }
            else if (UmbracoApp.Common.HasValue(specificNodeId) === true) {
                base.NavigateByNodeId(specificNodeId);
            }
        });
    };
    BaseController.prototype.ReloadTreeNodesByIds = function (nodeIds) {
        var _this = this;
        this.DependencyContext.EntityResource.getByIds(nodeIds, "Document")
            .then(function (documents) {
            _.each(documents, function (document) {
                _this.DependencyContext.NavigationService.syncTree({ tree: "content", path: document.path, forceReload: false, activate: false })
                    .then(function (syncArgs) {
                    _this.DependencyContext.NavigationService.reloadNode(syncArgs.node);
                });
            });
        });
    };
    BaseController.prototype.NavigateByNodeId = function (id) {
        var path = "/content/content/edit/";
        var nodeId = UmbracoApp.Common.HasValue(id) ? id : this.GetSelectedNodeId();
        var pathToNavigate = path + nodeId;
        this.DependencyContext.Location.path(pathToNavigate);
    };
    BaseController.prototype.GetCurrentNode = function () {
        return this.DependencyContext.AppState.getMenuState("currentNode");
    };
    BaseController.prototype.SelectSpecificNavTab = function (tabPosition) {
        var navTabs = $(".umb-nav-tabs > li:not(.hide)"), currentActiveNavTab = $(".umb-nav-tabs > li.active");
        currentActiveNavTab.removeClass("active");
        $(navTabs[tabPosition]).addClass("active");
    };
    BaseController.prototype.activateTabPanes = function (tabPosition) {
        var tabPanes = $('div.tab-content').find(".tab-pane"), currentActiveTabPane = $('div.tab-content').find(".tab-pane.active");
        currentActiveTabPane.removeClass("active");
        $(tabPanes[tabPosition]).addClass("active");
    };
    BaseController.prototype.ToggleNavigation = function () {
        var isNavigationShown = this.DependencyContext.AppState.getGlobalState("showNavigation");
        if (isNavigationShown) {
            this.DependencyContext.AppState.setGlobalState("showNavigation", false);
            $("#contentwrapper").css("left", "80px");
        }
        else {
            this.DependencyContext.AppState.setGlobalState("showNavigation", true);
            $("#contentwrapper").css("left", "440px");
        }
    };
    BaseController.prototype.NavigateByNodePath = function (path) {
        this.SyncTreeByPath(path, null, true, false, false);
    };
    return BaseController;
}());
var UrlModule;
(function (UrlModule) {
    var UrlService = (function (_super) {
        __extends(UrlService, _super);
        function UrlService(dependencyContext) {
            _super.call(this, dependencyContext);
            this.PluginKey = "UrlService";
        }
        UrlService.prototype.GetMediaMaintenance = function () {
            return "/media/tabs";
        };
        UrlService.prototype.GetMediaFolderStructure = function () {
            return "/media/folder/structure";
        };
        UrlService.prototype.GetAllFoldersByType = function (folderType) {
            return "/media/folders/folderType/" + folderType + "/all";
        };
        UrlService.prototype.GetAllEmptyFoldersByType = function (folderType) {
            return "/media/folders/folderType/" + folderType + "/empty";
        };
        UrlService.prototype.GetFoldersWithFilesAddedInLastNDays = function (days) {
            return "/media/folders/old/days/" + days;
        };
        UrlService.prototype.GetAllImages = function () {
            return "/media/images/all";
        };
        UrlService.prototype.GetImageReferencesById = function (imageId) {
            return "/media/images/references/imageId/" + imageId;
        };
        UrlService.prototype.GetAllImagesByPuprose = function (purpose) {
            return "/media/images/purpose/" + purpose;
        };
        UrlService.$Inject = ["dependencyContext"];
        return UrlService;
    }(BaseService));
    UrlModule.UrlService = UrlService;
})(UrlModule || (UrlModule = {}));
app.service("urlService", UrlModule.UrlService);
