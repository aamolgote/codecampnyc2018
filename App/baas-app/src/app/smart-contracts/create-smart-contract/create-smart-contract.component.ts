import { Component, OnInit } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { SmartContract } from 'src/app/models/smart-contracts.model';
import { Form, NgForm } from '@angular/forms';
import { AccountManagementService } from 'src/app/account-management/account-management.service';
import { UserDltAccount } from 'src/app/models/user-dlt-account';
import { Router } from '@angular/router';
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
    private accountMgmtService: AccountManagementService,
    private router: Router) {
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
    this.smartContractService.createSmartContract(this.smartContract, this.fileToBeUploaded)
      .subscribe((smartContract: SmartContract) => {
        smartContractForm.form.reset();
        console.log("Navigate to root..");
        this.router.navigate(['/']);
        console.log("Navigate to root done.....");

      });
  }

  navigateBack() {
    console.log("Navigate to root..");
    this.router.navigate(['/']);
    console.log("Navigate to root done.....");
  }

  onFileUploaded(filesList: File[]) {
    console.log("file to be uploaded...");
    if (filesList && filesList.length) {
      let file: File = filesList[0];
      if (file) {
        this.fileToBeUploaded = file;
      }
    }
  }
}
