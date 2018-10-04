import { Injectable, Inject } from '@angular/core';
import { Http, Headers, Response, RequestOptions, RequestOptionsArgs } from '@angular/http';
import { APP_CONFIG } from './../app.config';
import { IAppConfig } from './../iapp.config';
import { Observable, of, pipe } from 'rxjs';
import { map, takeUntil, catchError } from 'rxjs/operators'
import { DltBlock } from '../models/smart-contracts.model';
import { HttpClient } from '@angular/common/http';
@Injectable()
export class BlockExplorerService {
  private baseUrl: string;
  constructor(
    private http: HttpClient,
    @Inject(APP_CONFIG) private config: IAppConfig) {
    this.baseUrl = this.config.apiEndpointBaseUrl;
  }

  getRecentBlocks(numberOfBlocks: number): Observable<DltBlock[]>{
    let apiUrl = this.baseUrl + "api/blocks/recent?numberofblocks=" + numberOfBlocks;
    return this.http.get<DltBlock[]>(apiUrl)
      .pipe(
        catchError(this.handleError('getRecentBlocks', []))
      );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
        console.error(error);
        return of(result as T)
    }
}

}
