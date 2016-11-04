/**
 * Tweaks Module definition
 */
module TweaksModule {

    /**
     * Image model
     */
    export interface IImageModel {
        Id: number;
        Name: string;
        Metadata: {
            FilePath: string;
        }
    }

    /**
     * Media Response model
     */
    export interface IMediaResponseModel {
        Total: number;
        MediaFiles: IImageModel[];
    }
}