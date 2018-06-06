import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Lunch } from '@app/core/Models/lunch.model';
import { MealType } from '@app/core/Models/meal-type.model';
import { MenuService } from './menu.service';

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

  constructor(
    private menuService: MenuService,
    private router: Router
  ) {
    this.isLoading = true;
  }

  ngOnInit() {
    this.menuService.getNextWeekDates(new Date()).subscribe(dates => {
      this.dates = dates;
      const dateFrom = dates[0];
      const dateTo = dates[dates.length - 1];
      this.menuService.getMealTypes().subscribe(mealTypes => {
        this.mealTypes = mealTypes;
        this.menuService.getMeals().subscribe(meals => {
          this.mealTypes = this.menuService.sortMeals(meals, mealTypes);
          this.menuService.getLunches(dateFrom, dateTo).subscribe(lunches => {
            this.menuService.fillLunches(this.dates, this.mealTypes, lunches).subscribe(menuLunches => {
              this.lunches = menuLunches;
              this.lunchesLoaded = Promise.resolve(true);
              this.isLoading = false;
            });
          });
        });
      });
    });
  }

  Save() {
    this.menuService.BatchSave(this.lunches.filter(x => !!x.mealId)).subscribe(() => {
      this.isLoading = true;
      this.ngOnInit();
    });
  }

  getLunchByDateAndType(date: Date, mealType: MealType) {
    return this.lunches.find(x => x.typeId === mealType.id && x.date.toDateString() === date.toDateString());
  }
}
