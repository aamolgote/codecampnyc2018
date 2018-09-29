import { Component, OnInit } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { SmartContract } from 'src/app/models/smart-contracts.model';
import { Form, NgForm } from '@angular/forms';
import { AccountManagementService } from 'src/app/account-management/account-management.service';
import { UserDltAccount } from 'src/app/models/user-dlt-account';
@Component({
  selector: 'create-smart-contract',
  templateUrl: './create-smart-contract.component.html',
  styleUrls: ['./create-smart-contract.component.css']
})
export class CreateSmartContractComponent implements OnInit {
  smartContract: SmartContract;
  displayAdvanceOption: boolean = true;
  fileToBeUploaded: File;
  submitted = false;
  accounts: UserDltAccount[];
  constructor(private smartContractService: SmartContractService,
    private accountMgmtService: AccountManagementService) {
    this.smartContract = new SmartContract();
  }

  ngOnInit() {
    this.accountMgmtService.getUserAccounts()
      .subscribe((accounts: UserDltAccount[]) => {
        this.accounts = accounts;
      });
  }

  onSubmit(smartContractForm: NgForm) {
    this.submitted = true;
    this.smartContract = smartContractForm.value;
  }

  onFileUploaded(filesList: File[]) {
    if (filesList && filesList.length) {
      let file: File = FileList[0];
      if (file) {
        this.fileToBeUploaded = file;
      }
    }
  }
}
