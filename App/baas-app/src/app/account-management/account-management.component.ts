import { Component } from '@angular/core';
import { UserDltAccount } from '../models/user-dlt-account';
import { AccountManagementService } from './account-management.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'account-management',
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.css']
})
export class AccountManagementComponent {
  accounts: UserDltAccount[];

  constructor(private accountService: AccountManagementService,
    private router: Router, private activatedRoute: ActivatedRoute) {

  }

  ngOnInit(){
    this.accountService.getUserAccounts()
      .subscribe((userAccounts: UserDltAccount[])=>{
        this.accounts = userAccounts;
      });
  }
}
