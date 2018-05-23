import { Component, OnInit, Input,Output, EventEmitter } from '@angular/core';
import { SelectorContext } from '@angular/compiler';
import { Lunch } from '@app/lunch/lunch.model';

@Component({
    selector: 'app-lunch-tile',
    templateUrl: 'lunch-tile.component.html',
    styleUrls: ['lunch-tile.component.scss']
})

export class LunchTileComponent {
    @Input() lunch: Lunch;
  @Output()
  lunchSelected: EventEmitter<Lunch> = new EventEmitter<Lunch>(); //creating an output event
  constructor() { }
  ngOnInit() {
}
    select() {
        this.lunch.isSelected = !this.lunch.isSelected;
        this.lunchSelected.emit(this.lunch);
    }
}
