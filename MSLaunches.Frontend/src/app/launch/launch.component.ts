import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';

import { LaunchService } from './launch.service';
import { Launch } from '@app/launch/launch.model';
import { groupBy, Dictionary } from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-home',
  templateUrl: './launch.component.html',
  styleUrls: ['./launch.component.scss']
})
export class LaunchComponent implements OnInit {
  launchesByWeek: Dictionary<Launch[]>;
  launches: Array<Launch>;
  isLoading: boolean;

  constructor(private launchService: LaunchService) { }

  ngOnInit() {
    this.isLoading = true;
    this.launchService.getLaunches(new Date(2018, 1, 1), new Date(2018, 1, 0))
      .pipe(finalize(() => { this.isLoading = false; }))
      .subscribe(launches => {
        this.launchesByWeek = groupBy(launches, (result: Launch) =>
          moment(result.date, 'DD/MM/YYYY').startOf('isoWeek'));
      });
  }

  private groupByWeek(value: Launch) {
      const d = Math.floor(value.date.getTime() / (1000 * 60 * 60 * 24 * 7));
      this.launchesByWeek[d] = this.launchesByWeek[d] || [];
      this.launchesByWeek[d].push(value);
  }
}
