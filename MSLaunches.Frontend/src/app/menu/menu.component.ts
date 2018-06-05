import { Component, OnInit, Input } from '@angular/core';
import { filter, finalize, mergeMap } from 'rxjs/operators';

import { MenuService } from './menu.service';

import { MealGrouped } from '@app/core/Models/meal-grouped.model';
import { MealType } from '@app/core/Models/meal-type.model';
import { Meal } from '@app/core/Models/meal.model';
import * as moment from 'moment';
import * as _ from 'lodash';
import { Lunch } from '@app/core/Models/lunch.model';
import { LunchComponent } from '@app/lunch/lunch.component';
import { LunchService } from '@app/lunch/lunch.service';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  dates: Array<Date>;
  mealTypes: Array<MealType>;
  lunches: Array<Lunch>;
  isLoading: boolean;
  lunchesLoaded: Promise<boolean>;

  constructor(private menuService: MenuService) {
    this.isLoading = true;
  }

  ngOnInit() {
    this.menuService.getNextWeekDates(new Date()).subscribe(dates => {
      this.dates = dates;
      const dateFrom = dates[0];
      const dateTo = dates[dates.length - 1];
      this.menuService.getMealTypes().subscribe(mealTypes => {
        this.mealTypes = mealTypes;
        this.menuService.getLunches(dateFrom, dateTo).subscribe(lunches => {
          this.menuService.fillLunches(this.dates, this.mealTypes, lunches).subscribe(menuLunches => {
            this.lunches = menuLunches;
            this.lunchesLoaded = Promise.resolve(true);
            this.isLoading = false;
          });
        });
      });
    });
  }

  Save() {
    this.menuService.BatchSave(this.lunches.filter(x => !!x.mealId)).subscribe();
  }

  getLunchByDateAndType(date: Date, mealType: MealType) {
    return this.lunches.find(x => x.typeId === mealType.id && x.date.toDateString() === date.toDateString());
  }
}
