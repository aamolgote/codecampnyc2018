import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TabsModule, PaginationModule, CollapseModule, BsModalService, ModalModule } from 'ngx-bootstrap';
import { AppComponent } from './app.component';
import { APP_CONFIG, AppConfig } from './app.config';
import { SmartContractListingComponent } from './smart-contracts/smart-contract-listing/smart-contract-listing.component';
import { AccountManagementComponent } from './account-management/account-management.component';
import { BlockExplorerComponent } from './block-explorer/block-explorer.component';
import { AccountManagementService } from './account-management/account-management.service';
import { BlockExplorerService } from './block-explorer/block-explorer.service';
import { SmartContractService } from './smart-contracts/smart-contract.service';
import { CreateSmartContractComponent } from './smart-contracts/create-smart-contract/create-smart-contract.component';
import { FileDragDropUploadComponent } from './file-drag-drop-upload/file-drag-drop-upload.component';
import { SmartContractInstancesComponent } from './smart-contracts/smart-contract-instances/smart-contract-instances.component';
import { CreateSmartContractInstanceComponent } from './smart-contracts/create-smart-contract-instance/create-smart-contract-instance.component';
import { SmartContractInitiateActionComponent } from './smart-contracts/smart-contract-initiate-action/smart-contract-initiate-action.component';
const appRoutes: Routes = [
  {
    path: '',
    component: SmartContractListingComponent
  },
  {
    path: 'smartcontract/create',
    component: CreateSmartContractComponent
  },
  {
    path: 'accountmanagement/listing',
    component: AccountManagementComponent
  },
  {
    path: 'blockexplorer/listing',
    component: BlockExplorerComponent
  },
  {
    path: 'smartcontract/instances/deploy/:id',
    component: CreateSmartContractInstanceComponent
  },
  {
    path: 'smartcontract/instances/:id/action/:instanceId',
    component: SmartContractInitiateActionComponent
  },
  {
    path: 'smartcontract/instances/:id',
    component: SmartContractInstancesComponent
  }
 

]
@NgModule({
  declarations: [
    AppComponent,
    SmartContractListingComponent,
    AccountManagementComponent,
    BlockExplorerComponent,
    CreateSmartContractComponent,
    FileDragDropUploadComponent,
    SmartContractInstancesComponent,
    CreateSmartContractInstanceComponent,
    SmartContractInitiateActionComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
    CollapseModule.forRoot(),
    ModalModule.forRoot(),
    RouterModule.forRoot(appRoutes, { enableTracing: false, useHash: true })
  ],
  providers: [,
    AccountManagementService,
    BlockExplorerService,
    { provide: APP_CONFIG, useValue: AppConfig },
    SmartContractService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
