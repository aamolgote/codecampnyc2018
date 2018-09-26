import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap';
import { AppComponent } from './app.component';
import { APP_CONFIG, AppConfig } from './app.config';
import { SmartContractListingComponent } from './smart-contracts/smart-contract-listing/smart-contract-listing.component';
import { AccountManagementComponent } from './account-management/account-management.component';
import { BlockExplorerComponent } from './block-explorer/block-explorer.component';
import { AccountManagementService } from 'src/app/account-management/account-management.service';
import { BlockExplorerService } from 'src/app/block-explorer/block-explorer.service';
const appRoutes: Routes = [
  {
    path: '',
    component: SmartContractListingComponent
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
    BlockExplorerComponent
  ],
  imports: [
    BrowserModule,
    TabsModule.forRoot(),
    RouterModule.forRoot(appRoutes, { enableTracing: false, useHash: true })
  ],
  providers: [,
    AccountManagementService,
    BlockExplorerService,
    { provide: APP_CONFIG, useValue: AppConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
