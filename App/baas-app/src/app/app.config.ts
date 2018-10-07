import { InjectionToken } from "@angular/core";
import {IAppConfig} from './iapp.config';
export let APP_CONFIG = new InjectionToken("app.config");

export const AppConfig: IAppConfig = {    
    apiEndpointBaseUrl : "http://localhost:3338/"   
    //apiEndpointBaseUrl : "http://13.68.141.17/baasapi/"   
};
