import { Component, OnInit } from '@angular/core';
import { DltBlock } from '../models/smart-contracts.model';
import { BlockExplorerService } from './block-explorer.service';
import { PageChangedEvent } from 'ngx-bootstrap';

@Component({
  selector: 'block-explorer',
  templateUrl: './block-explorer.component.html',
  styleUrls: ['./block-explorer.component.css']
})
export class BlockExplorerComponent implements OnInit {
  public dltBlocks: DltBlock[];
  private pagedDltBlocks: DltBlock[];
  private startItem: number = 0;
  private endItem: number = 10;
  private timerId: any;
  public autoRefresh: Boolean = true;
  private refreshInterval: number = 5000;
  selected: string;
  searchFlag: Boolean = false;

  constructor(private blockExplorerService: BlockExplorerService) {

  }

  ngOnInit() {
    this.bindData();
    if (this.autoRefresh) {
      this.timerId = setInterval(() => {
        this.bindData();
      }, this.refreshInterval);
    }
  }

  ngOnDestroy(): void {
    if (this.timerId) {
      clearInterval(this.timerId);
    }
  }

  pageChnaged(event: PageChangedEvent) {
    this.startItem = (event.page - 1) * event.itemsPerPage;
    this.endItem = event.page * event.itemsPerPage;
    this.pagedDltBlocks = this.dltBlocks.slice(this.startItem, this.endItem);
  }

  private bindData() {
    let blockDictionary: any = {};
    if (this.dltBlocks && this.dltBlocks.length) {
      this.dltBlocks.forEach((dltBlock) => {
        blockDictionary[dltBlock.blockNumber] = dltBlock;
      });
    }

    this.blockExplorerService.getRecentBlocks(100)
      .subscribe((dltBlocks: DltBlock[]) => {
        this.dltBlocks = dltBlocks;
        if (this.dltBlocks && this.dltBlocks.length) {
          this.dltBlocks.forEach((dltBlock) => {
            if (blockDictionary[dltBlock.blockNumber]) {
              dltBlock.isCollapsed = blockDictionary[dltBlock.blockNumber].isCollapsed;
            }
            else {
              dltBlock.isCollapsed = true;
            }
          });
        }
        this.pagedDltBlocks = this.dltBlocks.slice(this.startItem, this.endItem);
      });
  }

  onChange(event: any) {
    if (!this.autoRefresh && this.timerId) {
      clearInterval(this.timerId);
    }
    else if (this.autoRefresh) {
      if (this.timerId) {
        clearInterval(this.timerId);
      }
      this.timerId = setInterval(() => {
        this.bindData();
      }, this.refreshInterval);
    }
  }
}
