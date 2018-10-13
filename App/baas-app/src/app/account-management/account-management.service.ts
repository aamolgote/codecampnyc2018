import { Injectable, Inject } from '@angular/core';
import { APP_CONFIG } from './../app.config';
import { IAppConfig } from './../iapp.config';
import { UserDltAccount } from '../models/user-dlt-account';
import { Observable, of, throwError, Subject, pipe } from 'rxjs';
import { map, takeUntil, catchError } from 'rxjs/operators'
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AccountManagementService {
  private accountMgmtServiceBaseUrl: string;
  private baseUrl: string;
  constructor(
    private http: HttpClient,
    @Inject(APP_CONFIG) private config: IAppConfig) {
    this.baseUrl = config.apiEndpointBaseUrl;
  }

  getUserAccounts(): Observable<UserDltAccount[]> {
    let apiUrl = this.baseUrl + "api/useraccounts";
    return this.http.get<UserDltAccount[]>(apiUrl)
      .pipe(
        catchError(this.handleError('', []))
      );

  }
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T)
    }
  }
}
