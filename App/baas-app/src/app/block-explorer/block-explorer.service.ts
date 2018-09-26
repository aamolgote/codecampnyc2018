import { Injectable, Inject } from '@angular/core';
import { Http, Headers, Response, RequestOptions, RequestOptionsArgs } from '@angular/http';
import { APP_CONFIG } from './../app.config';
import { IAppConfig } from './../iapp.config';
@Injectable()
export class BlockExplorerService {
  private blockExplorerServiceBaseUrl: string;
  constructor(
    private http: Http,
    @Inject(APP_CONFIG) private config: IAppConfig) {
    this.blockExplorerServiceBaseUrl = this.config.apiEndpointBaseUrl + 'api/companies/';
  }
}
