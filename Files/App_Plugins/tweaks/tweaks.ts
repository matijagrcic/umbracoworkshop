/// <reference path="../../scripts/typings/angularjs/angular.d.ts" />
/// <reference path="../../scripts/typings/umbraco/umbraco.d.ts" />
/// <reference path="../../scripts/typings/underscore/underscore.d.ts" />

module umbraco {

    enum ViewType {
        Dialogs,
        Directives,
        Overlays
    }

    enum UmbracoApiController {
        Media
    }

    export function getViewUrlByTypeAndName(viewType: ViewType, viewName: string) {
        switch (viewType) {
            case ViewType.Dialogs:
                return `views/common/dialogs/${viewName}.html`;
            case ViewType.Directives:
                return `views/directives/html/${viewName}.html`;
            case ViewType.Overlays:
                return `views/common/overlays/mediapicker/${viewName}.html`;
        }
    }

    export function getApiUrlByControllerAndAction(controller: UmbracoApiController, action: string) {
        switch (controller) {
            case UmbracoApiController.Media:
                return `/umbraco/backoffice/UmbracoApi/Media/${action}.html`;
        }
    }

    export class api {
        static media_getChildren = getApiUrlByControllerAndAction(UmbracoApiController.Media, "GetChildren");
    }

    export class dialogs {
        static mediaPicker = getViewUrlByTypeAndName(ViewType.Overlays, "mediapicker");
    }

    export class directives {
        static umb_photo_folder = getViewUrlByTypeAndName(ViewType.Directives, "mediaPicker/umb-photo-folder");
    }
}

module tweak {

    function getViewUrl(viewName: string) {
        return `/App_Plugins/tweaks/views/${viewName}.html`;
    }

    export class views {
        static mediaPicker = getViewUrl("media-picker");
    }

}

interface IUmbracoInterceptor<T> {
    request: (config: ng.IRequestConfig) => void;
    response: (response: ng.IHttpPromiseCallbackArg<T>) => void;
}

angular.module('umbraco.services').config(["$httpProvider", ($httpProvider: ng.IHttpProvider) => {

    function processUrl(requestUrl: string, compareUrl: string) {
        return requestUrl.indexOf(compareUrl) === 0;
    }

    function replaceMediaPickerWithCustomPanel(config: ng.IRequestConfig) {
        if (processUrl(config.url, umbraco.dialogs.mediaPicker)) {
            config.url = tweak.views.mediaPicker;
        }
    }

    function replacePanelsTweak($q: ng.IQService): IUmbracoInterceptor<umb.services.IMediaPickerModel> {
        return {
            request: (config) => {
                replaceMediaPickerWithCustomPanel(config);
                return config || $q.when(config);
            },
            response: (response) => {
                return response;
            }
        };
    }

    $httpProvider.interceptors.push(replacePanelsTweak);

}]);