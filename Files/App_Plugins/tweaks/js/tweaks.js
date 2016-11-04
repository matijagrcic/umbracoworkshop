var umbraco;
(function (umbraco) {
    var ViewType;
    (function (ViewType) {
        ViewType[ViewType["Dialogs"] = 0] = "Dialogs";
        ViewType[ViewType["Directives"] = 1] = "Directives";
        ViewType[ViewType["Overlays"] = 2] = "Overlays";
    })(ViewType || (ViewType = {}));
    var UmbracoApiController;
    (function (UmbracoApiController) {
        UmbracoApiController[UmbracoApiController["Media"] = 0] = "Media";
    })(UmbracoApiController || (UmbracoApiController = {}));
    function getViewUrlByTypeAndName(viewType, viewName) {
        switch (viewType) {
            case ViewType.Dialogs:
                return "views/common/dialogs/" + viewName + ".html";
            case ViewType.Directives:
                return "views/directives/html/" + viewName + ".html";
            case ViewType.Overlays:
                return "views/common/overlays/mediapicker/" + viewName + ".html";
        }
    }
    umbraco.getViewUrlByTypeAndName = getViewUrlByTypeAndName;
    function getApiUrlByControllerAndAction(controller, action) {
        switch (controller) {
            case UmbracoApiController.Media:
                return "/umbraco/backoffice/UmbracoApi/Media/" + action + ".html";
        }
    }
    umbraco.getApiUrlByControllerAndAction = getApiUrlByControllerAndAction;
    var api = (function () {
        function api() {
        }
        api.media_getChildren = getApiUrlByControllerAndAction(UmbracoApiController.Media, "GetChildren");
        return api;
    }());
    umbraco.api = api;
    var dialogs = (function () {
        function dialogs() {
        }
        dialogs.mediaPicker = getViewUrlByTypeAndName(ViewType.Overlays, "mediapicker");
        dialogs.linkPicker = getViewUrlByTypeAndName(ViewType.Dialogs, "linkPicker");
        return dialogs;
    }());
    umbraco.dialogs = dialogs;
    var directives = (function () {
        function directives() {
        }
        directives.umb_photo_folder = getViewUrlByTypeAndName(ViewType.Directives, "mediaPicker/umb-photo-folder");
        return directives;
    }());
    umbraco.directives = directives;
})(umbraco || (umbraco = {}));
var tweak;
(function (tweak) {
    function getViewUrl(viewName) {
        return "/App_Plugins/tweaks/views/" + viewName + ".html";
    }
    var views = (function () {
        function views() {
        }
        views.mediaPicker = getViewUrl("media-picker");
        views.linkPicker = getViewUrl("link-picker");
        return views;
    }());
    tweak.views = views;
})(tweak || (tweak = {}));
angular.module('umbraco.services').config(["$httpProvider", function ($httpProvider) {
        function processUrl(requestUrl, compareUrl) {
            return requestUrl.indexOf(compareUrl) === 0;
        }
        function replaceMediaPickerWithCustomPanel(config) {
            if (processUrl(config.url, umbraco.dialogs.mediaPicker)) {
                config.url = tweak.views.mediaPicker;
            }
        }
        function replaceLinkPickerWithCustomPanel(config) {
            if (processUrl(config.url, umbraco.dialogs.linkPicker)) {
                config.url = tweak.views.linkPicker;
            }
        }
        function replacePanelsTweak($q) {
            return {
                request: function (config) {
                    replaceMediaPickerWithCustomPanel(config);
                    replaceLinkPickerWithCustomPanel(config);
                    return config || $q.when(config);
                },
                response: function (response) {
                    return response;
                }
            };
        }
        $httpProvider.interceptors.push(replacePanelsTweak);
    }]);
