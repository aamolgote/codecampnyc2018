import { Component } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { SmartContract } from 'src/app/models/smart-contracts.model';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'smart-contract-listing',
  templateUrl: './smart-contract-listing.component.html',
  styleUrls: ['./smart-contract-listing.component.css']
})
export class SmartContractListingComponent implements OnInit {
  title = 'baas-app';
  smartContracts: SmartContract[];
  constructor(private smartContractService: SmartContractService, 
    private router: Router, private acitvatedRoute: ActivatedRoute){
    
  }

  ngOnInit(){
    console.log("Smart contract listing....");
    this.smartContractService.getSmartContracts()
    .subscribe((smartContracts: SmartContract[])=>{
      this.smartContracts = smartContracts;
      console.log(this.smartContracts);
      this.smartContracts.forEach((smartContract)=>{
        if (smartContract.name.indexOf("Mortgage") > -1){
          smartContract.imagePath = "./assets/images/mortgage.png";
        }
        else if (smartContract.name.indexOf("Blockchain") > -1){
          smartContract.imagePath = "./assets/images/blockchain.png";
        }
        else if (smartContract.name.indexOf("Market Place") > -1){
          smartContract.imagePath = "./assets/images/marketplace.png";
        }
        else if (smartContract.name.indexOf("Asset Transfer") > -1){
          smartContract.imagePath = "./assets/images/assettransfer.jpeg";
        }
        else{
          smartContract.imagePath = "./assets/images/smartcontract.png";
        }
      });
    })
  }

  createContract(){
    this.router.navigate(['/smartcontract/create'])
  }

}
