/**
 * Backoffice Module definition
 */
module BackofficeModule {

    /**
     * Provides an interface that defines the Plugin Model
     */
    export interface IPlugin {
        Folder: string;
        Controller: string;
        Tabs: ITabsConfiguration;
        Values: IPluginValuesConfiguration;
    }

    /**
     * Provides an interface that defines the Tabs Configuration Model
     */
    export interface ITabsConfiguration {
        All: ITab[];
    }

    /**
     * Provides an interface that defines the Plugin Values Configuration Model
     */
    export interface IPluginValuesConfiguration {
        All: IPluginValues[];
    }

     /**
     * Provides an interface that defines the Tab Model
     */
    export interface ITab {
        Id: string;
        Label: string;
        Description: string;
        Template: string;
    }

     /**
     * Provides an interface that defines the Plugin Values Model
     */
    export interface IPluginValues {
        Name: string;
        Description: string;
        Value: string;
    }

     /**
     * Provides an interface that defines the Tab Model
     */
    export interface ITabModels<T> {
        TabModel: T|T[];
    }

    export interface ITabModelsMultiple<T, T1> {
        TabModel: T|T1;
    }
}