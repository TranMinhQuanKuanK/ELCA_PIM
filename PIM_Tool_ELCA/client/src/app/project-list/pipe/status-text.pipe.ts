import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'statusText'
})
export class StatusTextPipe implements PipeTransform {

  transform(value: string): string {
    switch (value) {
      case "NEW":
        return "New";
      case "PLA":
        return "Planned";
      case "INP":
        return "In Progress";
      case "FIN":
        return "Finished";
    }
    return "";
  }
}
