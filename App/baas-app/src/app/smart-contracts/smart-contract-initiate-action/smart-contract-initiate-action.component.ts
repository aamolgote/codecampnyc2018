import { Component } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { SmartContract } from 'src/app/models/smart-contracts.model';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'smart-contract-initiate-action',
  templateUrl: './smart-contract-initiate-action.component.html',
  styleUrls: ['./smart-contract-initiate-action.component.css']
})
export class SmartContractInitiateActionComponent implements OnInit {
  title = 'baas-app';
  smartContracts: SmartContract[];
  constructor(private smartContractService: SmartContractService, 
    private router: Router, private acitvatedRoute: ActivatedRoute){
    
  }

  ngOnInit(){
    
  }

 
}
