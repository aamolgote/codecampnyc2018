import { Component } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { SmartContract, ContractFunctionInfo, UserDltAccount, SmartContractInstance, DeployedInstance, ExecuteFunctionPayload, SmartContractTransaction, InputParamsInfo } from 'src/app/models/smart-contracts.model';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountManagementService } from '../../account-management/account-management.service';

@Component({
  selector: 'smart-contract-initiate-action',
  templateUrl: './smart-contract-initiate-action.component.html',
  styleUrls: ['./smart-contract-initiate-action.component.css']
})
export class SmartContractInitiateActionComponent implements OnInit {
  smartContractInstanceId: number;
  accounts: UserDltAccount[] = [];
  smartContract: SmartContract;
  smartContractInstance: SmartContractInstance;
  smartContractDeployedInstance: DeployedInstance;
  contractFunctionInfoList: ContractFunctionInfo[] = [];
  executeFuncPayload: ExecuteFunctionPayload;
  displayLoader: Boolean = false;
  displayLoaderFunc: string;
  showErrorMessage: Boolean = false;
  errorMsg: any;

  constructor(private smartContractService: SmartContractService,
    private router: Router, private acitvatedRoute: ActivatedRoute,
    private accountService: AccountManagementService) {

  }


  executeFunctionOnContract(funcInfo: ContractFunctionInfo): void {
    this.displayLoader = true;
    this.executeFuncPayload = new ExecuteFunctionPayload();
    this.executeFuncPayload.function = funcInfo.functionName;
    this.executeFuncPayload.smartContractDeployedInstanceId = this.smartContractInstanceId;
    this.executeFuncPayload.transactionUser = funcInfo.transactionUser;
    this.executeFuncPayload.parameters = [];
    for (var i = 0; i < funcInfo.inputParamsList.length; ++i) {
      var inputValue = funcInfo.inputParamsList[i].inputValue;
      // this.executeFuncPayload.parameters.push(inputValue);
      if (funcInfo.inputParamsList[i].type === "string" || funcInfo.inputParamsList[i].type === "address") {
        this.executeFuncPayload.parameters.push(inputValue);
      }
      else if (funcInfo.inputParamsList[i].type === "bool") {
        this.executeFuncPayload.parameters.push(inputValue);
      }
      else if (funcInfo.inputParamsList[i].type.indexOf("int") > -1) {
        this.executeFuncPayload.parameters.push(parseInt(inputValue));
      }
    }
    if (funcInfo.stateMutability == "view") {
      this.smartContractService.executeReadFunction(this.executeFuncPayload)
        .subscribe((readFunctionResponse: string) => {
          funcInfo.readFunctionResponse = readFunctionResponse;
          this.displayLoader = false;
        });
    }
    else if (funcInfo.stateMutability == "nonpayable") {
      this.smartContractService.executewriteFunction(this.executeFuncPayload)
        .subscribe((writeFunctionResponse: SmartContractTransaction) => {
          funcInfo.writeFunctionResponse = writeFunctionResponse;
          this.displayLoader = false;
        },
          (error: any) => {
            console.log(error);
            this.errorMsg = error;
            this.showErrorMessage = true;
            this.displayLoader = false;
          }
        );
    }
  }

  readAbi(): void {
    let abi = this.smartContract.abi;
    let abiJson = JSON.parse(abi);
    let contractFunctionInfoList: ContractFunctionInfo[] = [];
    for (var i = 0; i < abiJson.length; i++) {
      if (abiJson[i].type !== "event" && abiJson[i].type !== "constructor") {
        let funcInfo = new ContractFunctionInfo();
        funcInfo.type = abiJson[i].type;
        funcInfo.functionName = abiJson[i].name;
        funcInfo.stateMutability = abiJson[i].stateMutability;
        var inputsList: InputParamsInfo[] = [];
        for (var j = 0; j < abiJson[i].inputs.length; j++) {
          let inputofFunc = new InputParamsInfo();
          inputofFunc.name = abiJson[i].inputs[j].name;
          inputofFunc.type = abiJson[i].inputs[j].type;
          inputsList.push(inputofFunc);
        }
        funcInfo.inputParamsList = inputsList;
        funcInfo.belongsToContractInstance = this.smartContractInstanceId;
        contractFunctionInfoList.push(funcInfo);
      }
    }
    if (this.smartContract) {
      let dictionary: any = {};
      if (this.smartContract && this.smartContract.functions && this.smartContract.functions.length) {
        this.smartContract.functions.forEach(smartContractFunction => {
          dictionary[smartContractFunction.functionName] = smartContractFunction.sequence;
        });
        if (contractFunctionInfoList && contractFunctionInfoList.length) {
          contractFunctionInfoList.forEach(funcInfo => {
            if (dictionary[funcInfo.functionName]) {
              funcInfo.sequence = dictionary[funcInfo.functionName];
            }
          });
          this.contractFunctionInfoList = contractFunctionInfoList.sort((a, b) => {
            return a.sequence - b.sequence;
          });
        }
      }
    }
  }

  displayOnPaneForFunction(functionPane: string) {
    this.displayLoader = true;
    this.displayLoaderFunc = functionPane;
    this.showErrorMessage = false;
  }
  getAccounts() {
    this.accountService.getUserAccounts()
      .subscribe((userAccounts: UserDltAccount[]) => {
        this.accounts = userAccounts;
      });
  }

  getSmartContract() {
    this.smartContractInstance = this.smartContractService.getSavedSmartContract();
    if (this.smartContractInstance && this.smartContractInstance.smartContract) {
      this.smartContract = this.smartContractInstance.smartContract;
      this.initializeSmartContractInstance();
    }

    if (!this.smartContract) {
      let id = +this.acitvatedRoute.snapshot.paramMap.get('id');
      this.smartContractService.getDeployedInstanceListing(id)
        .subscribe((deployedInstance: SmartContractInstance) => {
          this.smartContractInstance = deployedInstance;
          if (deployedInstance && deployedInstance.smartContract) {
            this.smartContract = deployedInstance.smartContract; this.readAbi();
            this.initializeSmartContractInstance();
          }
        });
    }
    else {
      this.readAbi();
    }
  }

  initializeSmartContractInstance() {
    if (this.smartContractInstanceId && this.smartContractInstance && this.smartContractInstance.smartContractDeployedInstanceItems) {
      this.smartContractDeployedInstance = this.smartContractInstance.smartContractDeployedInstanceItems.filter(deployedInstance => deployedInstance.smartContractInstanceId === this.smartContractInstanceId)[0];
    }
  }

  ngOnInit(): void {
    this.smartContractInstanceId = +this.acitvatedRoute.snapshot.paramMap.get('instanceId');
    console.log(this.smartContractInstanceId);
    this.getAccounts();
    this.getSmartContract();
    console.log(this.smartContractInstance);
  }

  navigateBack() {
    let contractId = +this.acitvatedRoute.snapshot.paramMap.get('id');
    let url = './smartcontract/instances/' + contractId;
    this.router.navigate([url]);
  }
}
