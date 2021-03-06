import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';
import { Meal } from '../core/Models/meal.model';
import { MealType } from '../core/Models/meal-type.model';
import * as moment from 'moment';
import { Lunch } from '@app/core/Models/lunch.model';

@Injectable()
export class MenuService {
  constructor(private httpClient: HttpClient) { }

  private credentialsKey = 'credentials';
  private routes = {
    mealTypes(): string {
      return '/mealtypes';
    },
    meals(): string {
      return '/meals';
    },
    saveLunches(): string {
      return '/lunches/batchsave';
    },
    getLunchesBetweenDates(dateFrom: Date, dateTo: Date): string {
      return `/lunches/BetweenDates/${dateFrom.toISOString()}/${dateTo.toISOString()}`;
    }
  };

  getMeals(): Observable<Array<Meal>> {
    return this.httpClient.get(this.routes.meals())
      .pipe(map(this.mapMeals));
  }

  getMealTypes(): Observable<Array<MealType>> {
    return this.httpClient.get(this.routes.mealTypes())
      .pipe(map(this.mapToArrayOfMealType.bind(this)));
  }

  getLunches(dateFrom: Date, dateTo: Date): Observable<Array<Lunch>> {
    return this.httpClient
      .get(this.routes.getLunchesBetweenDates(dateFrom, dateTo))
      .pipe(map(this.mapToArrayOfLunches.bind(this)));
  }

  fillLunches(dates: Array<Date>, mealTypes: Array<MealType>, lunches: Array<Lunch>): Observable<Array<Lunch>> {
    dates.forEach(date => {
      mealTypes.forEach(mealType => {
        const lunch = lunches.find(x => x.typeId === mealType.id && x.date.toDateString() === date.toDateString());
        if (!lunch) {
          let newLunch: Lunch;
          newLunch = {
            date: date,
            mealId: '',
            typeId: mealType.id
          };
          lunches.push(newLunch);
        }
      });
    });
    return of(lunches);
  }

  getNextWeekDates(date: Date): Observable<Array<Date>> {
    return of(this.GetNextWeekDates(date));
  }

  BatchSave(lunches: Array<Lunch>): Observable<string> {
    const userId = JSON.parse(sessionStorage.getItem(this.credentialsKey)).userId;
    return this.httpClient
      .post(this.routes.saveLunches(), lunches.map(this.mapToBackend))
      .pipe(map((a: any) => a.LunchId));
  }

  sortMeals(meals: Array<Meal>, mealTypes: Array<MealType>): Array<MealType> {
    mealTypes.forEach(mealType => {
      mealType.meals = meals.filter(meal => meal.typeId === mealType.id);
    });

    return mealTypes;
  }

  private mapToArrayOfMealType(body: Array<any>): Array<MealType> {
    let result: Array<MealType> = new Array<MealType>();
    result = body.map(this.mapToMealType);
    return result;
  }

  private mapToMealType(body: any): MealType {
    let result: MealType;
    result = {
      name: body.description,
      id: body.id,
      meals: body.meals
    };
    return result;
  }

  private mapToLunch(body: any): Lunch {
    let result: Lunch;
    result = {
      mealId: body.meal.id,
      typeId: body.meal.typeId,
      date: new Date(body.date)
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

  private mapToArrayOfLunches(body: Array<any>): Array<Lunch> {
    let result: Array<Lunch> = new Array<Lunch>();
    result = body.map(this.mapToLunch);
    return result;
  }

  private mapMeals(meals: Array<any>): Array<Meal> {
    const result = new Array<Meal>();
    meals.forEach(element => {
      result.push({
        id: element.id,
        name: element.name,
        typeDescriptcion: element.type.description,
        typeId: element.type.id,
      });
    });
    return result;
  }

  private mapToBackend(lunch: Lunch): any {
    return {
      mealid: lunch.mealId,
      date: lunch.date
    };
  }
}
