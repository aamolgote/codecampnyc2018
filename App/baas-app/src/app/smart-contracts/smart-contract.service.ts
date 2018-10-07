import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IAppConfig } from './../iapp.config'
import { APP_CONFIG } from './../app.config'
import { Observable, of, throwError, Subject, pipe } from 'rxjs';
import { map, takeUntil, catchError } from 'rxjs/operators'
import { SmartContractToBeDeployed, DeployedInstance, SmartContract, SmartContractInstance, SmartContractTransaction, ExecuteFunctionPayload } from 'src/app/models/smart-contracts.model';
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

    executeReadFunction(executeFunctionPayload: ExecuteFunctionPayload): Observable<string> {
        let apiUrl = this.baseUrl + "api/smartcontract/instance/executereadfunction";
        return this.http.post<string>(apiUrl, executeFunctionPayload)
            .pipe(
                catchError(this.handleError('executeReadFunction', null))
            )
    }
    executewriteFunction(executeFunctionPayload: ExecuteFunctionPayload): Observable<SmartContractTransaction> {
        let apiUrl = this.baseUrl + "api/smartcontract/instance/executewritefunction";
        return this.http.post<SmartContractTransaction>(apiUrl, executeFunctionPayload)
            .pipe(
                catchError(this.handleError('executewriteFunction', null))
            )
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
    private smartContractInstance: SmartContractInstance;
    saveSmartContract(smartContractInstance: SmartContractInstance) {
        this.smartContractInstance = smartContractInstance;
        console.log("Save Smart contract");
    }

    getSavedSmartContract(): SmartContractInstance {
        return this.smartContractInstance;
        console.log("Get Smart contract");
    }
    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
            console.error(error);
            return of(result as T)
        }
    }
}