//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Data {
    public class SampleAssignmentService : IAssignmentService {
        public Task<List<Assignment>> GetAssignmentsAsync ()
        {
            return Database.GetConnection ()
                .Table<Assignment> ()
                .OrderBy (a => a.Priority)
                .ToListAsync ();
        }

        public Task<List<Item>> GetItemsAsync ()
        {
            return Database.GetConnection ()
                .Table<Item> ()
                .OrderBy (i => i.Name)
                .ToListAsync ();
        }

        public Task<List<AssignmentItem>> GetItemsForAssignmentAsync (Assignment assignment)
        {
            return Database.GetConnection ()
                .QueryAsync<AssignmentItem> (@"
                    select AssignmentItem.*, Item.Number, Item.Name
                    from AssignmentItem
                    inner join Item
                    on Item.ID = AssignmentItem.ID
                    where AssignmentItem.Assignment = ?
                    order by Item.Name",
                    assignment.ID);
        }

        public Task<List<Labor>> GetLaborForAssignmentAsync (Assignment assignment)
        {
            return Database.GetConnection ()
                .Table<Labor> ()
                .Where (l => l.Assignment == assignment.ID)
                .ToListAsync ();
        }

        public Task<List<Expense>> GetExpensesForAssignmentAsync (Assignment assignment)
        {
            return Database.GetConnection ()
                .Table<Expense> ()
                .Where (e => e.Assignment == assignment.ID)
                .ToListAsync ();
        }

        public Task<int> SaveAssignment (Assignment assignment)
        {
            return Database.GetConnection ()
                .UpdateAsync (assignment);
        }

        public Task<int> SaveAssignmentItem (AssignmentItem assignmentItem)
        {
            if (assignmentItem.ID == 0)
                return Database.GetConnection ().InsertAsync (assignmentItem);
            else
                return Database.GetConnection ().UpdateAsync (assignmentItem);
        }

        public Task<int> SaveLabor (Labor labor)
        {
            if (labor.ID == 0)
                return Database.GetConnection ().InsertAsync (labor);
            else
                return Database.GetConnection ().UpdateAsync (labor);
        }

        public Task<int> SaveExpense (Expense expense)
        {
            if (expense.ID == 0)
                return Database.GetConnection ().InsertAsync (expense);
            else
                return Database.GetConnection ().UpdateAsync (expense);
        }

        public Task<int> DeleteAssignment (Assignment assignment)
        {
            return Database.GetConnection ()
                .DeleteAsync (assignment);
        }

        public Task<int> DeleteAssignmentItem (AssignmentItem assignmentItem)
        {
            return Database.GetConnection ()
                .DeleteAsync (assignmentItem);
        }

        public Task<int> DeleteLabor (Labor labor)
        {
            return Database.GetConnection ()
                .DeleteAsync (labor);
        }

        public Task<int> DeleteExpense (Expense expense)
        {
            return Database.GetConnection ()
                .DeleteAsync (expense);
        }

        public Task<int> SaveTimerEntry (TimerEntry entry)
        {
            //If the ID is zero, it's an insert, also set the ID to 1
            if (entry.ID == 0) {
                entry.ID = 1;
                return Database.GetConnection ().InsertAsync (entry);
            } else {
                return Database.GetConnection ().UpdateAsync (entry);
            }
        }

        public Task<int> DeleteTimerEntry (TimerEntry entry)
        {
            return Database.GetConnection ().DeleteAsync (entry);
        }

        public Task<TimerEntry> GetTimerEntryAsync ()
        {
            //Just return the first row
            return Database.GetConnection ().FindAsync<TimerEntry> (_ => true);
        }
    }
}