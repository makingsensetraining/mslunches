import { Component, OnInit, Input } from '@angular/core';
import { filter, finalize } from 'rxjs/operators';

import { LunchService } from './lunch.service';
import { Lunch, DailyTypedLunches, WeeklyLunches } from '@app/lunch/lunch.model';
import * as moment from 'moment';
import * as _ from 'lodash';

@Component({
  selector: 'app-lunch',
  templateUrl: './lunch.component.html',
  styleUrls: ['./lunch.component.scss']
})
export class LunchComponent implements OnInit {
  @Input() canBeSelected: boolean;

  lunches: Array<WeeklyLunches>;
  isLoading: boolean;

  constructor(private lunchService: LunchService) {
  }

  ngOnInit() {
    this.isLoading = true;
    this.lunchService.getUserLunches()
      .pipe(finalize(() => { this.isLoading = false; }))
      .subscribe(lunches => {
        this.lunches = this.lunchService.mapToWeekly(lunches);
      });

    if (this.canBeSelected === undefined) {
      this.canBeSelected = true;
    }
  }

  setLunchSelected(lunch: Lunch) {
    let weeklyLunches: WeeklyLunches;
    let lunchesByDay: DailyTypedLunches;
    const lunchDate: Date = new Date(lunch.date);
    const startOfTheWeek: Date = this.firstDayOfTheWeek(lunch.date, 'YYYY-MM-DD').toDate();

    weeklyLunches = this.lunches.find(l =>
      l.date.getDate() === startOfTheWeek.getDate()
    );

    lunchesByDay = weeklyLunches.lunches.find(
      l => l.date.getDate() === lunchDate.getDate()
    );

    lunchesByDay.lunches.forEach(l => {
      if (l.id !== lunch.id) {
        l.isSelected = false;
      }
    });

    if (lunch.isSelected) {
      this.lunchService.save(lunch).subscribe(
        a => lunch.userLunchId = a,
        (err: any) => lunch.isSelected = false);
    } else {
      this.lunchService.delete(lunch).subscribe({error: a => lunch.isSelected = true});
    }
  }

  private firstDayOfTheWeek(date: Date, format?: moment.MomentFormatSpecification): moment.Moment {
    if (!format) {
      format = 'DD/MM/YYYY';
    }
    return moment(date, format).startOf('isoWeek');
  }
}
