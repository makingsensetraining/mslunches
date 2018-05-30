import { Component, OnInit, Input } from '@angular/core';
import { filter, finalize } from 'rxjs/operators';

import { MenuService } from './menu.service';


import { MealGrouped } from '@app/core/Models/meal-grouped.model';
import { MealType } from '@app/core/Models/meal-type.model';
import { Meal } from '@app/core/Models/meal.model';
import * as moment from 'moment';
import * as _ from 'lodash';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  dates: Array<Date>;
  mealTypes: Array<MealType>;
  mealGrouped: Array<MealGrouped>;
  isLoading: boolean;

  constructor(private menuService: MenuService) {
    this.isLoading = true;
  }

  ngOnInit() {
    this.menuService
      .getNextWeekDates(new Date())
      .subscribe(dates => {
        this.dates = dates;
      });

    this.menuService
      .getMealTypes()
      .subscribe(mealTypes => {
        this.mealTypes = mealTypes;
      });

    this.isLoading = false;
  }
}
