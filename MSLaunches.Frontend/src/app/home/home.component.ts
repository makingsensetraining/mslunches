import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { tupleTypeAnnotation } from 'babel-types';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  isLoading: boolean;
  isModifiable: boolean;

  constructor() { }

  ngOnInit() {
    this.isModifiable = false;
    this.isLoading = false;
  }

  modifySelection() {
    this.isModifiable = !this.isModifiable;
  }

}
