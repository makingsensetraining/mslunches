import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SelectorContext } from '@angular/compiler';
import { UserLunch } from '@app/core/Models/user-lunch.model';


@Component({
    selector: 'app-lunch-tile',
    templateUrl: 'lunch-tile.component.html',
    styleUrls: ['lunch-tile.component.scss']
})

export class LunchTileComponent {
    @Input() lunch: UserLunch;
    @Input() canBeSelected: UserLunch;
    @Output() lunchSelected: EventEmitter<UserLunch>;
    constructor() {
        this.lunchSelected = new EventEmitter<UserLunch>();
    }

    select() {
        this.lunch.isSelected = !this.lunch.isSelected;
        this.lunchSelected.emit(this.lunch);
    }
}
