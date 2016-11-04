/// <reference path="../../model.ts" />

/**
 * Media Maintenance Module definition
 */
module MediaMaintenanceModule {

    /**
     * Provides an interface that defines the Archive Maintenance Model
     */
    export interface IMediaMaintenance {
        Plugin: BackofficeModule.IPlugin;
    }

    export interface IMediaModel extends IBaseMediaModel {
    }

    export interface IFolderModel extends IBaseMediaModel {
        Folders: IFolderModel[];
        Images: IMediaModel[];
    }

    export interface IBaseMediaModel {
        Path: string;
        NodeId: number;
        Id: number;
    }

    export interface IMediaReference extends IBaseMediaModel {

    }

    /**
    * Media Response model
    */
    export interface IMediaResponseModel<T> {
        Total: number;
        MediaFiles: T[];
    }
}