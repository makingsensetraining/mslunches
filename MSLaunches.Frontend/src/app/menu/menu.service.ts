import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';
import { Meal } from '../core/Models/meal.model';
import { MealType } from '../core/Models/meal-type.model';
import * as moment from 'moment';

@Injectable()
export class MenuService {
  constructor(private httpClient: HttpClient) {}

  private credentialsKey = 'credentials';
  private routes = {
    getMealTypes(): string {
      return `/mealtypes`;
    }
  };

  getMealTypes(): Observable<Array<MealType>> {
    return this.httpClient.get(this.routes.getMealTypes()).pipe(map(this.mapToArrayOfMealType.bind(this)));
  }

  getNextWeekDates(date: Date): Observable<Array<Date>> {
    return of(this.GetNextWeekDates(date));
  }

  private mapToArrayOfMeal(body: Array<any>): Array<Meal> {
    let result: Array<Meal> = new Array<Meal>();
    result = body.map(this.mapToMeal);
    return result;
  }

  private mapToMeal(body: any): Meal {
    let result: Meal;
    result = {
      name: body.name,
      id: body.id,
      typeId: body.mealType.id,
      typeDescriptcion: body.mealType.id
    };
    return result;
  }

  private mapToArrayOfMealType(body: Array<any>): Array<MealType> {
    let result: Array<MealType> = new Array<MealType>();
    result = body.map(this.mapToMealType);
    return result;
  }

  mapToMealType(body: any): MealType {
    let result: MealType;
    result = {
      name: body.description,
      id: body.id,
      meals: body.meals
    };
    return result;
  }

  private GetNextWeekDates(today: Date): Array<Date> {
    today.setDate(today.getDate() + 7);
    let startOfTheweek: moment.Moment;
    const dates: Array<Date> = new Array<Date>();
    startOfTheweek = moment(today, 'DD/MM/YYYY').startOf('isoWeek');
    for (let i = 0; i < 5; i++) {
      dates.push(startOfTheweek.toDate());
      startOfTheweek.add(1, 'days');
    }
    return dates;
  }
}
