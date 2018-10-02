import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IAppConfig } from './../iapp.config'
import { APP_CONFIG } from './../app.config'
import { Observable, of, throwError, Subject, pipe } from 'rxjs';
import { map, takeUntil, catchError } from 'rxjs/operators'
import { SmartContractToBeDeployed, DeployedInstance, SmartContract, SmartContractInstance, SmartContractTransaction } from 'src/app/models/smart-contracts.model';
import { Response } from '@angular/http';
@Injectable()
export class SmartContractService {
    private baseUrl: string;
    constructor(private http: HttpClient, @Inject(APP_CONFIG) private config: IAppConfig) {
        this.baseUrl = config.apiEndpointBaseUrl;
    }

    deployContract(smartContractToBeDeployed: SmartContractToBeDeployed): Observable<DeployedInstance> {
        let apiUrl = this.baseUrl + "api/smartcontract/deploy";
        return this.http.post<DeployedInstance>(apiUrl, smartContractToBeDeployed)
            .pipe(
                map((res: Response) => {
                    let deployedInstance = res.json();
                    return deployedInstance;
                }),
                catchError(this.handleError('deployContract'))
            );
    }


    getSmartContracts(): Observable<SmartContract[]> {
        console.log("GetSmartContracts Service.....");
        let apiUrl = this.baseUrl + "api/smartcontracts";
        return this.http.get<SmartContract[]>(apiUrl)
            .pipe(
                catchError(this.handleError('', []))
            );

    }



    getDeployedInstanceListing(smartContractId: number): Observable<SmartContractInstance> {
        let apiUrl = this.baseUrl + "api/smartcontract/instances?smartContractId=" + smartContractId;
        return this.http.get<SmartContractInstance>(apiUrl)
            .pipe(
                catchError(this.handleError('getDeployedInstanceListing', null))
            );
    }

    getSmartContractDeployedInstanceTransactions(smartContractDeployedInstanceId: number): Observable<SmartContractTransaction[]> {
        let apiUrl = this.baseUrl + "api/smartcontract/instance/transactions?smartContractDeployedInstanceId" + smartContractDeployedInstanceId;
        return this.http.get<SmartContractTransaction[]>(apiUrl)
            .pipe(
                map((res: Response) => {
                    let smartContractDeployedInstanceTransactions = res.json();
                    return smartContractDeployedInstanceTransactions;
                }),
                catchError(this.handleError('getSmartContractDeployedInstanceTransactions'))
            );
    }

    createSmartContract(smartcontract: SmartContract, filetoBeUploaded: File): Observable<SmartContract> {
        let apiUrl = this.baseUrl + "api/smartcontract";
        let formData: FormData = new FormData();
        formData.append("smartContractModelData", JSON.stringify(smartcontract));

        if (filetoBeUploaded && filetoBeUploaded.name) {
            formData.append('uploadFile', filetoBeUploaded, filetoBeUploaded.name);
        }
        return this.http.post<SmartContract>(apiUrl, formData)
            .pipe(
                catchError(this.handleError('an error occured while invoking createSmartContract', null))
            );
    }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            console.error(error);
            return of(result as T)
        }
    }
}