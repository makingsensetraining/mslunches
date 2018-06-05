import { Component, Input, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';

import * as moment from 'moment';

import { DailyTypedLunches } from '@app/core/Models/daily-typed-lunches.model';
import { UserLunch } from '@app/core/Models/user-lunch.model';
import { WeeklyLunches } from '@app/core/Models/weekly-lunches.model';
import { LunchService } from '@app/lunch/lunch.service';

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

  setLunchSelected(lunch: UserLunch) {
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
