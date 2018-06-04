import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SelectorContext } from '@angular/compiler';
import { Meal } from '@app/core/Models/meal.model';
import { MealType } from '@app/core/Models/meal-type.model';
import { Lunch } from '@app/core/Models/lunch.model';


@Component({
    selector: 'app-menu-tile',
    templateUrl: 'menu-tile.component.html',
    styleUrls: ['menu-tile.component.scss']
})

export class MenuTileComponent {
    @Input() meals: Array<Meal>;
    @Input() date: Date;
    @Output() lunchCreated: EventEmitter<Lunch>;
    constructor() {
        this.lunchCreated = new EventEmitter<Lunch>();
    }

    SaveLunch(meal: Meal) {
        let result: Lunch;
        result = {
            mealId: meal.id,
            date: this.date
        };
    }
}


