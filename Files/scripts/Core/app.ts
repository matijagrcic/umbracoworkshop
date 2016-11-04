/// <reference path="../typings/angularjs/angular.d.ts" />
/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/underscore/underscore.d.ts" />
/// <reference path="DependencyContext.ts" />
/// <reference path="../typings/umbraco/umbraco.d.ts" />
/// <reference path="../typings/moment/moment.d.ts" />
/// <reference path="../typings/accounting/accounting.d.ts" />

/**
 * Provides the umbraco angular app interface
 */
interface IUmbracoApp extends ng.IModule {

}

/**
 * Provides the base angular scope interface definition
 */
interface IBaseScope extends ng.IScope {
    model: any;
}

/**
 * Declare the umbraco angular app
 */
declare var app: IUmbracoApp;

/**
 * Get a reference to the angular module - this will always need to be umbraco to create the necessary hook
 */
app = angular.module("umbraco");

/**
 * Umbraco module definition
 */
module UmbracoModule {

    /**
    * Provides a class for common methods shared methods
    */
    export class Common {

        /**
       * Provides the default UI format for dates
       * for displaying use "| date" Angular filter so for example Mar 2, 2015 will be result
       */
        DefaultUIDateFormat = "MM/DD/YYYY";

        /**
        * Provides the default API format for dates
        */
        DefaultApiFormat = "YYYY-MM-DD";

        /**
        * Globally implement a string formattting method
         * @param {format : string} The string formatter
         * @param {replacements : string[]} The array of string parameters
        */
        Format = (format: string, ...replacements: any[]) => {
            return format.replace(/{(\d+)}/g,(match, number) => typeof replacements[number] != 'undefined'
                ? replacements[number]
                : match).toString();
        }

        /**
        * Check if value is undefined
        * @param value The value.
        */
        IfUndefined<T>(value: T): boolean {
            var isUndefined = (typeof value === 'undefined');
            return isUndefined;
        }

        /**
        * Check if value is undefined or null
        * @param value The value.
        */
        HasValue<T>(value: T): boolean {
            return value !== null && !this.IfUndefined<T>(value);
        }

        /**
       * Gets the value from the nullable number and if empty default to the passed value
        * @param value The value.
        * @param defaultVal The default value.
       */
        GetValueOrDefault(value?: number, defaultVal: string = ""): string {
            return this.HasValue<number>(value) ? value.toString() : defaultVal;
        }

        /**
        * Delete value from array by index
         * @param array The array.
         * @param index The index.
        */
        DeleteValueFromArrayByIndex<T>(array: T[], index: number): void {
            array.splice(index, 1);
        }

        /*
         * Check if string is undefined or empty
         * @param value The value to check.
         */
        IsStringNullOrUndefined(value: string): boolean {
            return (!value || value.length === 0);
        }

        /**
       * Get property name by value
        * @param object The object.
        * @param propertyValue The property value.
       */
        GetPropertyNameByValue<T>(object: T, propertyValue: any): string {
            for (var property in object) {
                if (object.hasOwnProperty(property) && object[property] == propertyValue) {
                    return property;
                }
            }
        }


        /**
       * Delete object from array by specific property value
        * @param array The array.
        * @param propertyName The property that contains desired value.
        * @param propertyValue The property value you are looking for.
       */
        DeleteObjectFromArrayBySpecificPropertyValue<T>(array: T[], propertyValue: any): void {
            array.forEach((current: T, index: number) => {
                var propertyName = this.GetPropertyNameByValue(current, propertyValue);
                if (this.IfUndefined(propertyName) === false && current[propertyName] == propertyValue) {
                    this.DeleteValueFromArrayByIndex(array, index);
                }
            });
        }

        /**
      * Get object in array by specific property value
       * @param array The array.
       * @param propertyName The property that contains desired value.
        * @param propertyValue The property value you are looking for.
      */
        GetObjectFromArrayBySpecificPropertyValue<T>(array: T[], propertyValue: any): T {
            var foundObject;
            array.forEach((current: T, index: number) => {
                var propertyName = this.GetPropertyNameByValue(current, propertyValue);
                if (this.IfUndefined(propertyName) === false && current[propertyName] == propertyValue) {
                    foundObject = current;
                    return;
                }
            });
            return foundObject;
        }

        /**
        * Push value to array
         * @param array The array.
         * @param element The element.
        */
        PushValueToArray<T>(array: T[], value: T): void {
            array.push(value);
        }

        /**
        * Replace value in array by index
         * @param array The array.
         * @param index The index.
         * @param value The value.
        */
        ReplaceValueInArrayByIndex<T>(array: T[], index: number, value: T): void {
            array[index] = value;
        }

        GetNumberFromString(value: string): number {
            value = value.replace(/\D/g, '');
            return parseInt(value);
        }

        /**
        * Open a url in a new broweer tab
         * @param url The url to open.
        */
        OpenInNewTab(url: string): void {
            var win = window.open(url, '_blank');
            win.focus();
        }

        /**
        * Formats a date as per the api string format
         * @param url The url to open.
        */
        FormatApiDate(date: string): string {
            var formattedDate = moment(date).format(UmbracoApp.Common.DefaultApiFormat);
            return formattedDate;
        }

        /**
        * Formats the money as currency
         * @param value The value to format.
        */
        FormatMoney(value: any, isAlreadyFormatted: boolean = false): string {
            var amountWithCurrency;
            if (isAlreadyFormatted) {
                if (value) {
                    amountWithCurrency = UmbracoApp.Common.Format("${0}", value);
                } else {
                    amountWithCurrency = UmbracoApp.Common.Format("${0}", 0.00);
                }
            } else {
                var amount = (value / 100);
                amountWithCurrency = accounting.formatMoney(amount);
            }
            return amountWithCurrency;
        }
    }

    /**
    * Provides the main class for accessing any global functions or providers
    */
    export class UmbracoMain {
        /**
        * Common functionality
        */
        Common: UmbracoModule.Common;

        /**
         * Creates a new instance of the Common object
         * @constructor
         */
        constructor() {
            this.Common = new UmbracoModule.Common();
        }
    }
}

var UmbracoApp: UmbracoModule.UmbracoMain = new UmbracoModule.UmbracoMain();
