import { Component } from '@angular/core';
import { SmartContractService } from './../smart-contract.service';
import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { SmartContract, SmartContractInstance, DeployedInstance } from 'src/app/models/smart-contracts.model';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef, PageChangedEvent } from 'ngx-bootstrap';

@Component({
  selector: 'smart-contract-instances',
  templateUrl: './smart-contract-instances.component.html',
  styleUrls: ['./smart-contract-instances.component.css']
})
export class SmartContractInstancesComponent implements OnInit {
  deployedInstance: SmartContractInstance;
  pagedSmartContractInstanceItems: DeployedInstance[];
  private startItem: number = 0;
  private endItem: number = 10;
  modalRef: BsModalRef;

  constructor(private smartContractService: SmartContractService,
    private router: Router,
    private acitvatedRoute: ActivatedRoute) {

  }

  ngOnInit() {
    let id = +this.acitvatedRoute.snapshot.paramMap.get("id");
    console.log("Smart contract listing....");
    this.smartContractService.getDeployedInstanceListing(id)
      .subscribe((smartContractInstance: SmartContractInstance) => {
        console.log(this.deployedInstance);
        this.deployedInstance = smartContractInstance;
        console.log(this.deployedInstance);
        this.pagedSmartContractInstanceItems = this.deployedInstance.smartContractDeployedInstanceItems.slice(this.startItem, this.endItem);
        this.pagedSmartContractInstanceItems.reverse();
      });
  }

  pageChanged(event: PageChangedEvent) {
    this.startItem = (event.page - 1) * event.itemsPerPage;
    this.endItem = event.page * event.itemsPerPage;
    this.pagedSmartContractInstanceItems = this.deployedInstance.smartContractDeployedInstanceItems.slice(this.startItem, this.endItem);
    this.pagedSmartContractInstanceItems.reverse();

  }

  saveSmartContractToService(smartContractInstance: SmartContractInstance, smartContractInstanceId: number){
    this.smartContractService.saveSmartContract(smartContractInstance);
    console.log(smartContractInstance);
    //[routerLink]="['./action/', instanceItem.smartContractInstanceId]"
    //this.router.navigate(['/action/', smartContractInstanceId]);
  }

  navigateBack() {
    this.router.navigate(['/']);
  }

  navigateToDeployContract(){
    console.log( this.deployedInstance.smartContract.smartContractId);
    this.router.navigate(['/smartcontract/instances/deploy', this.deployedInstance.smartContract.smartContractId]);
  }
}
