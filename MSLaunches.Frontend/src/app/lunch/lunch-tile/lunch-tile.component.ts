import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SelectorContext } from '@angular/compiler';
import { Lunch } from '@app/lunch/lunch.model';

@Component({
    selector: 'app-lunch-tile',
    templateUrl: 'lunch-tile.component.html',
    styleUrls: ['lunch-tile.component.scss']
})

export class LunchTileComponent implements OnInit {
    @Input() lunch: Lunch;
    @Input() canBeSelected: Lunch;
    @Output() lunchSelected: EventEmitter<Lunch>;
    constructor() {
        this.lunchSelected = new EventEmitter<Lunch>();
    }

    ngOnInit() {
        this.lunch.date = new Date(this.lunch.date);
        this.lunch.date.setHours(10);
        this.lunch.isSelectable =
            this.lunch.isSelectable && this.lunch.date.getTime() > Date.now();
    }

    select() {
        this.lunch.isSelected = !this.lunch.isSelected;
        this.lunchSelected.emit(this.lunch);
    }
}
