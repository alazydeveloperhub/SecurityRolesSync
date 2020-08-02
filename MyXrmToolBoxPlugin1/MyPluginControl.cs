using System;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using System.Web.UI.WebControls;
using Microsoft.Crm.Sdk.Messages;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using System.ComponentModel;

namespace SecurityRolesSync
{
    public partial class MyPluginControl : PluginControlBase
    {
        // Cache of users from CDS
       
        private Settings mySettings;
        private EntityCollection UserListCache { get; set; }

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }

            ExecuteMethod(PopulateDropdownsFormLoad);
       
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        private void GetUsers(String searchTxt, SearchType searchType)
        {
            var newUsersList = new EntityCollection();

            // Use cached list of users first 
            // If nothing is in the cache, get fresh list of users from CDS.

            var UsersList = UserListCache == null ? GetUserEntitiesCDS() : UserListCache;

            // Populate dropdowns with all users if search is blank. 
            // No need to filter

            if(searchTxt == "")
            {
                newUsersList = UsersList;
            }

            // Else, filter the user cache
            else
            {
                for (int i = 0; i < UsersList.Entities.Count; i++)
                {
                    var entity = UsersList.Entities[i];

                    var fullName = (entity["fullname"].ToString() + entity["systemuserid"].ToString()).ToLowerInvariant();

                    if (fullName.Contains(searchTxt.ToLowerInvariant()))
                    {
                        newUsersList.Entities.Add(entity);
                    }
                }
            }    
           
            // Populate the dropdowns UI

            switch (searchType)
            {
                case SearchType.Source:
                    PopulateSourceUsersDropdown(newUsersList);
                    break;
                case SearchType.Target:
                    PopulateTargetUsersDropdown(newUsersList);
                    break;
                default:
                    break;
            }
            
        }

        /// <summary>
        /// Retrieve all the users from CDS
        /// </summary>
        /// <returns></returns>
        private EntityCollection GetUserEntitiesCDS()
        {
            ConditionExpression condition1 = new ConditionExpression();
            condition1.AttributeName = "isdisabled";
            condition1.Operator = ConditionOperator.Equal;
            condition1.Values.Add("false");

            FilterExpression filter1 = new FilterExpression();
            filter1.Conditions.Add(condition1);

            QueryExpression query = new QueryExpression("systemuser");
            query.ColumnSet.AddColumns("systemuserid", "fullname");
            query.Criteria.AddFilter(filter1);

            return Service.RetrieveMultiple(query);
        }

        /// <summary>
        /// Get initial dropdown data and populate the dropdown UI controls.
        /// Populate the user list cache
        /// </summary>
        private void PopulateDropdownsFormLoad()
        {
          
            WorkAsync(new WorkAsyncInfo
            {
                Work = (worker, args) =>
                {
                    // get data from CDS

                    args.Result = GetUserEntitiesCDS();
                   
                },
                PostWorkCallBack = (args) => 
                {
                    NotifyAsyncError(args);

                    var result = args.Result as EntityCollection;

                    // update cache

                    UserListCache = result; 

                    // populate UI

                    PopulateSourceUsersDropdown(result);
                    PopulateTargetUsersDropdown(result);
                }
            });

        }

        private void NotifyAsyncError(RunWorkerCompletedEventArgs args)
        {
            if (args.Error != null)
            {
                MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateSourceUsersDropdown(EntityCollection UsersList)
        {
            WorkAsync(new WorkAsyncInfo
            {

                Work = (worker, args) =>
                {
                    var userEntities = UsersList;
                    args.Result = userEntities;

                },
                PostWorkCallBack = (args) =>
                {
                    NotifyAsyncError(args);

                    var result = args.Result as EntityCollection;

                    if (result != null)
                    {
                        var dropDownListCollection = new ListItemCollection();
                       
                        for (int i = 0; i < result.Entities.Count; i++)
                        {
                            dropDownListCollection.Add(new ListItem(result.Entities[i]["fullname"].ToString(), result.Entities[i]["systemuserid"].ToString()));
                          
                        }

                        Source.DataSource = dropDownListCollection;
                        Source.DisplayMember = "Text";
                        Source.ValueMember = "Value";

                    }
                }
            });
        }


        private void PopulateTargetUsersDropdown(EntityCollection UsersList)
        {
            WorkAsync(new WorkAsyncInfo
            {
          
                Work = (worker, args) =>
                {

                    if (UsersList == null)
                    {
                        UsersList = GetUserEntitiesCDS();
                    }

                    var userEntities = UsersList;

                    args.Result = userEntities;

                },
                PostWorkCallBack = (args) =>
                {
                    NotifyAsyncError(args);

                    var result = args.Result as EntityCollection;

                    if (result != null)
                    {
                      
                        var dropDownListCollection1 = new ListItemCollection();

                        for (int i = 0; i < result.Entities.Count; i++)
                        {
                           
                            dropDownListCollection1.Add(new ListItem(result.Entities[i]["fullname"].ToString(), result.Entities[i]["systemuserid"].ToString()));
                        }

                        Target.DataSource = dropDownListCollection1;
                        Target.DisplayMember = "Text";
                        Target.ValueMember = "Value";

                    }
                }
            });
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }

            UserListCache = null;
            ExecuteMethod(PopulateDropdownsFormLoad);
        }

        private void syncRoles_Click(object sender, EventArgs e)
        {
            Guid sourceUserId = new Guid(Source.SelectedValue.ToString());
            Guid targetUserId = new Guid(Target.SelectedValue.ToString());
            Exception er = null;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Syncing Security Roles from Source to Target...",
                Work = (w, ar) =>
                {
                    // This code is executed in another thread

                    try
                    {
                        List<Guid> sourceRoleIds;
                        Guid SourceBuId;
                        GetUserRoles(sourceUserId, out sourceRoleIds, out SourceBuId);

                        List<Guid> targetRoleIds;
                        Guid targetBuId;
                        GetUserRoles(targetUserId, out targetRoleIds, out targetBuId);

                        if (SourceBuId == targetBuId)
                        {
                            EntityReferenceCollection targetRoleCollection = new EntityReferenceCollection();
                            foreach (var tr in targetRoleIds)
                            {
                                targetRoleCollection.Add(new EntityReference("role", tr));
                            }
                            //clear all the sec roles of target user

                            Service.Disassociate(
                                            "systemuser",
                                            targetUserId,
                                            new Relationship("systemuserroles_association"),
                                            targetRoleCollection);
                        }
                        else
                        {

                            SetBusinessSystemUserRequest request = new SetBusinessSystemUserRequest();
                            request.BusinessId = SourceBuId;
                            request.UserId = targetUserId;
                            request.ReassignPrincipal = new EntityReference("systemuser", targetUserId);
                            SetBusinessSystemUserResponse response =
                            (SetBusinessSystemUserResponse)Service.Execute(request);
                        }

                        //Adding Bu and Roles to the target user

                        EntityReferenceCollection roleCollection = new EntityReferenceCollection();
                        foreach (var r in sourceRoleIds)
                        {
                            roleCollection.Add(new EntityReference("role", r));
                        }

                        Service.Associate("systemuser", targetUserId,
                                           new Relationship("systemuserroles_association"),
                                            roleCollection);

                        //Remove Old Teams from Targer User
                        List<Guid> targetUserTeamIds = GetUserTeams(targetUserId, Service);
                        Guid[] targetUser = new Guid[] { targetUserId };
                        foreach (Guid team in targetUserTeamIds)
                        {
                            RemoveMembersFromTeam(team, targetUser, Service);
                        }

                        //Adding teams from Source users to Target user
                        List<Guid> teamIds = GetUserTeams(sourceUserId, Service);

                        foreach (Guid team in teamIds)
                        {
                            AddMembersToTeam(team, targetUser, Service);
                        }
                    }
                    catch (Exception ex)
                    {
                        er = ex;
                    }
                },

                PostWorkCallBack = ar =>
                {
                   if (er!= null)
                    {
                        MessageBox.Show(er.Message);
                    }
                    else
                    {
                        MessageBox.Show($"Security Roles synced.");
                    }                   
                },
                AsyncArgument = null,
                // Progress information panel size
                MessageWidth = 340,
                MessageHeight = 150
            });

        }

        public List<Guid> GetUserTeams(Guid UserID, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("team");
            query.ColumnSet = new ColumnSet("teamid", "isdefault");
            LinkEntity link = query.AddLink("teammembership", "teamid", "teamid");
            link.LinkCriteria.AddCondition(new ConditionExpression("systemuserid", ConditionOperator.Equal, UserID));

            try
            {
                EntityCollection teams = service.RetrieveMultiple(query);
                List<Guid> teamIds = new List<Guid>(); ;
                foreach (Entity t in teams.Entities)
                {
                    if (t["isdefault"].ToString() == "False")
                    {
                        teamIds.Add(t.Id);
                    }
                }
                return teamIds;
            }
            catch (Exception ex)
            {
                // Do your Error Handling here
                throw ex;
            }
        }

        public static void AddMembersToTeam(Guid teamId, Guid[] membersId, IOrganizationService service)
        {
            AddMembersTeamRequest addRequest = new AddMembersTeamRequest();
            addRequest.TeamId = teamId;
            addRequest.MemberIds = membersId;
            service.Execute(addRequest);
        }

        private void GetUserRoles(Guid userid, out List<Guid> roleIds, out Guid buId)
        {
            QueryExpression qe = new QueryExpression("systemuserroles");
            qe.ColumnSet.AddColumns("systemuserid");
            qe.Criteria.AddCondition("systemuserid", ConditionOperator.Equal, userid);

            LinkEntity link1 = qe.AddLink("systemuser", "systemuserid", "systemuserid", JoinOperator.Inner);
            link1.Columns.AddColumns("fullname", "internalemailaddress", "businessunitid");
            LinkEntity link = qe.AddLink("role", "roleid", "roleid", JoinOperator.Inner);
            link.Columns.AddColumns("roleid", "name");
            EntityCollection results = Service.RetrieveMultiple(qe);

            roleIds = new List<Guid>();
            buId = new Guid();
            foreach (var c in results.Entities)
            {
                EntityReference businessunit = (EntityReference)(((AliasedValue)c["systemuser1.businessunitid"]).Value);
                buId = businessunit.Id;
                roleIds.Add(new Guid(((AliasedValue)c["role2.roleid"]).Value.ToString()));

            }
        }

        public static void RemoveMembersFromTeam(Guid teamId, Guid[] membersId, IOrganizationService service)
        {
            RemoveMembersTeamRequest removeRequest = new RemoveMembersTeamRequest();
            removeRequest.TeamId = teamId;
            removeRequest.MemberIds = membersId;
            service.Execute(removeRequest);
        }

        private void txtSearchSourceUser_TextChanged(object sender, EventArgs e)
        {
            GetUsers(txtSearchSourceUser.Text, SearchType.Source);
        }

        private void txtSearchSourceUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtSearchSourceUser.Text == "Search...")
            {
                txtSearchSourceUser.Text = "";
            }

        }

        private void txtServerTargetUser_TextChanged(object sender, EventArgs e)
        {
            GetUsers(txtSearchTargetUser.Text, SearchType.Target);
        }

        private void txtServerTargetUser_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtSearchTargetUser.Text == "Search...")
            {
                txtSearchTargetUser.Text = "";
            }
        }

        private void buttonAddNewUser_Click(object sender, EventArgs e)
        {
            string firstName = Prompt.ShowDialog("First Name", "The user's first name");
        }
    }
}