import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'file-drag-drop-upload',
  templateUrl: './file-drag-drop-upload.component.html',
  styleUrls: ['./file-drag-drop-upload.component.css']
})
export class FileDragDropUploadComponent implements OnInit {
  @Input() allowedExtensions: Array<string> = [];
  @Output() filesDraggedDroppedEmitter: EventEmitter<File[]> = new EventEmitter();
  uploadedFileName: string;
  constructor() {

  }
  ngOnInit() {

  }
  onFileUploadDragOver($event: any) {
    $event.preventDefault();
    $event.stopPropagation();
  }
  onFileUploadDragLeave($event) {
    $event.preventDefault();
    $event.stopPropagation();
  }
  onFileUploadDrop($event) {
    $event.preventDefault();
    $event.stopPropagation();

    let files = $event.dataTransfer.files;
    let validFiles: File[] = [];
    if (files.length > 0) {
      this.emitFilesUploaded(files);
    }
  }

  fileUploaded($event) {
    let files: File[] = $event.target.files;
    if (files.length > 0) {
      this.emitFilesUploaded(files);
    }
  }

  emitFilesUploaded(files: File[]) {
    let validFiles: File[] = [];
    for(let count = 0; count < files.length; count++){
      let file = files[count];
      let ext = file.name.split('.')[file.name.split('.').length - 1];
      if (this.allowedExtensions.lastIndexOf(ext) != -1){
        validFiles.push(file);
      }
    }
    
    if (validFiles && validFiles.length){
      this.uploadedFileName = validFiles[0].name;
      this.filesDraggedDroppedEmitter.emit(validFiles);
    }
  }
}