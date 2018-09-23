import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap';
import { AppComponent } from './app.component';
import { ContractListingComponent } from './contract-listing/contract-listing.component';
import { AccountManagementComponent } from './account-management/account-management.component';
import { BlockExplorerComponent } from './block-explorer/block-explorer.component';
const appRoutes: Routes = [
  {
    path: '',
    component: ContractListingComponent
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
    ContractListingComponent,
    AccountManagementComponent,
    BlockExplorerComponent
  ],
  imports: [
    BrowserModule,
    TabsModule.forRoot(),
    RouterModule.forRoot(appRoutes, { enableTracing: false, useHash: true})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
