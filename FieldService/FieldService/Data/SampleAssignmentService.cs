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

namespace FieldService.Data
{
    public class SampleAssignmentService : IAssignmentService
    {
        public Task<List<Assignment>> GetAssignments()
        {
            return Database.GetConnection()
                .Table<Assignment>()
                .OrderBy (a => a.Status)
                .ToListAsync();
        }

        public Task<List<Item>> GetItems()
        {
            return Database.GetConnection()
                .Table<Item>()
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public Task<List<AssignmentItem>> GetItemsForAssignment(Assignment assignment)
        {
            return Database.GetConnection()
                .QueryAsync<AssignmentItem>(@"
                    select AssignmentItem.* 
                    from AssignmentItem
                    inner join Item
                    on Item.ID = AssignmentItem.ID
                    where AssignmentItem.Assignment = ?
                    order by Item.Name",
                    assignment.ID);
        }
    }
}
