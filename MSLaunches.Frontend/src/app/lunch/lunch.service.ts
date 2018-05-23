import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map, mapTo } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';
import { tryStatement } from 'babel-types';
import { Lunch } from '@app/lunch/lunch.model';

@Injectable()
export class LunchService {
    constructor(private httpClient: HttpClient) {
    }


    getLaunches(startDate: Date, endDate: Date): Observable<Array<Lunch>> {
        return this.httpClient
            .get('/lunches')
            .pipe(map(this.mapToArrayOfLaunch.bind(this)));
        // return of(this.mapToArrayOfLaunch(null));
    }

    private mapToArrayOfLaunch(body: Array<any>): Array<Lunch> {
        let result: Array<Lunch> = new Array<Lunch>();

        result = body.map(this.mapToLaunch);

        return result;
    }

    private mapToLaunch(body: any): Lunch {
        let result: Lunch;
        result = {
            id: body.meal.id,
            description: body.meal.name,
            type: body.meal.mealType.description,
            date: body.date,
            isSelected: false,
            isSelectable: false
        };

        return result;
    }
}
