import { Component, OnInit, Input } from '@angular/core';
import { SelectorContext } from '@angular/compiler';
import { Lunch } from '@app/lunch/lunch.model';

@Component({
    selector: 'app-lunch-tile',
    templateUrl: 'lunch-tile.component.html',
    styleUrls: ['lunch-tile.component.scss']
})

export class LunchTileComponent {
    @Input() lunch: Lunch;

    select() {
        this.lunch.isSelected = !this.lunch.isSelected;
    }
}
