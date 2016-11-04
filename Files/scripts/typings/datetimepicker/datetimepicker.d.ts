﻿/// <reference path="../jquery/jquery.d.ts"/>

interface DateTimePickerConfig {

    pickDate: boolean;
    pickTime: boolean;
    useSeconds: boolean;
    format: string;
    icons: DateTimePickerIcons;                        
}

interface DateTimePickerIcons {
    time: string;
    date: string;
    up: string;
    down: string;
}

interface JQuery {

    datetimepicker(config?: DateTimePickerConfig): JQuery;

    datetimepicker(method: string, parameter?: any): JQuery;
   
}
