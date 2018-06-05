import { Component, EventEmitter, Input, Output, OnInit } from '@angular/core';

import { UserLunch } from '@app/core/Models/user-lunch.model';

@Component({
    selector: 'app-lunch-tile',
    templateUrl: 'lunch-tile.component.html',
    styleUrls: ['lunch-tile.component.scss']
})

export class LunchTileComponent implements OnInit {
    @Input() lunch: UserLunch;
    @Input() canBeSelected: UserLunch;
    @Output() lunchSelected: EventEmitter<UserLunch>;
    constructor() {
        this.lunchSelected = new EventEmitter<UserLunch>();
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
