import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap';
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
  }

]
@NgModule({
  declarations: [
    AppComponent,
    SmartContractListingComponent,
    AccountManagementComponent,
    BlockExplorerComponent,
    CreateSmartContractComponent,
    FileDragDropUploadComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
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
