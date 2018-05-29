import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SelectorContext } from '@angular/compiler';
import { Lunch } from '@app/core/Models/lunch.model';


@Component({
    selector: 'app-lunch-tile',
    templateUrl: 'lunch-tile.component.html',
    styleUrls: ['lunch-tile.component.scss']
})

export class LunchTileComponent {
    @Input() lunch: Lunch;
    @Input() canBeSelected: Lunch;
    @Output() lunchSelected: EventEmitter<Lunch>;
    constructor() {
        this.lunchSelected = new EventEmitter<Lunch>();
    }

    select() {
        this.lunch.isSelected = !this.lunch.isSelected;
        this.lunchSelected.emit(this.lunch);
    }
}
