import { Component, Input } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountManagementService } from './../../account-management/account-management.service';
import { DeployedInstance, SmartContractToBeDeployed, UserDltAccount, SmartContractInstance, InputParamsInfo } from '../../models/smart-contracts.model';
import { FormGroup, FormArray, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'create-smart-contract-instance',
  templateUrl: './create-smart-contract-instance.component.html',
  styleUrls: ['./create-smart-contract-instance.component.css']
})
export class CreateSmartContractInstanceComponent implements OnInit {
  displayLoader: boolean = false;
  deployedContract: DeployedInstance;
  deploymentForm: FormGroup;
  @Input() contractToBeDeployed: SmartContractToBeDeployed;
  contractId: number;
  accounts: UserDltAccount[] = [];
  deployedInstance: SmartContractInstance;
  constructorInputs: InputParamsInfo[] = [];
  constructorParamsFormArray: FormArray;
  constructor(private smartContractService: SmartContractService,
    private router: Router, private acitvatedRoute: ActivatedRoute,
    private accountService: AccountManagementService,
    private formBuilder: FormBuilder) {
    this.contractToBeDeployed = new SmartContractToBeDeployed();
    this.createForm();
  }

  createForm() {
    this.deploymentForm = this.formBuilder.group({
      deploymentData: ['', Validators.required],
      deployByUserLoginId: '',
      deployedInstanceDisplayName: '',
      constructorParams: this.formBuilder.array([])
    });
  }

  initConstructorParamsFields(): FormGroup {
    return this.formBuilder.group({
      name: ['', Validators.required]
    })
  }

  ngOnInit() {
    let id = parseInt(this.acitvatedRoute.snapshot.paramMap.get('id'));
    this.contractId = id;
    this.accountService.getUserAccounts()
      .subscribe((userAccounts: UserDltAccount[]) => {
        this.accounts = userAccounts;
      });

    this.smartContractService.getDeployedInstanceListing(id)
      .subscribe((deployedInstance: SmartContractInstance) => {
        this.deployedInstance = deployedInstance;

        let smartContract = deployedInstance.smartContract;

        let abi = JSON.parse(smartContract.abi);
        let constructorInfo;
        if (abi && abi.length) {
          abi.forEach(abiItem => {
            if (abiItem.type === "constructor") {
              constructorInfo = abiItem;
            }
          });

          if (constructorInfo && constructorInfo.inputs && constructorInfo.inputs.length) {
            constructorInfo.inputs.forEach(input => {
              let inputOfFunc = new InputParamsInfo();
              inputOfFunc.name = input.name;
              inputOfFunc.type = input.type;
              this.constructorInputs.push(inputOfFunc);
            });
          }

          if (this.constructorInputs && this.constructorInputs.length) {
            const control = <FormArray>this.deploymentForm.controls.constructorParams;

            this.constructorInputs.forEach(input => {
              control.push(this.initConstructorParamsFields());
            });
          }
        }
      });
  }

  saveContrctByForm() {
    const formModel = this.deploymentForm.value;

    const saveContract: SmartContractToBeDeployed = {
      smartContractId: this.contractId,
      deployByUserLoginId: formModel.deployByUserLoginId as string,
      deploymentData: [],
      deployedInstanceDisplayName: formModel.deployedInstanceDisplayName as string
    }

    if (formModel && formModel.constructorParams && formModel.constructorParams.length) {
      formModel.constructorParams.forEach((param, index) => {
        let paramType = this.constructorInputs[index].type;
        if (paramType === "string" || paramType === "address") {
          saveContract.deploymentData.push(param.name);
        }
        else if (paramType === "bool") {
          saveContract.deploymentData.push(param.name);
        }
        else if (paramType.indexOf("int") > -1) {
          saveContract.deploymentData.push(parseInt(param.name));
        }
      });
    }
    return saveContract;
  }

  onSelect(user: string) {
    this.contractToBeDeployed.deployByUserLoginId = user;
  }

  onSubmit() {
    this.contractToBeDeployed = this.saveContrctByForm();
    this.displayLoader = true;
    this.smartContractService.deployContract(this.contractToBeDeployed)
      .subscribe((deployedContract: DeployedInstance) => {
        this.deployedContract = deployedContract;
        this.displayLoader = false;
      });
  }

  navigateBack() {
    let url = "smartcontract/instances/" + this.contractId;
    this.router.navigate([url]);
  }
}
