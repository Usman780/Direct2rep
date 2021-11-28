using Direct2Rep.DAL;
using Direct2Rep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Direct2Rep.BL
{
    public class AdminBL
    {

        #region User
        public List<User> getUserList()
        {
            return new AdminDAL().getUsersList();
        }

        public User getUserById(int _id)
        {
            return new AdminDAL().getUserById(_id);
        }

        public bool AddUser(User _user)
        {
            if (_user.Name == null || _user.Email == null || _user.Password == null)

                return false;
            return new AdminDAL().AddUser(_user);
        }

        public bool UpdateUser(User _user)
        {
            if (_user.Name == null || _user.Email == null || _user.Password == null || _user.Role == null)

                return false;

            return new AdminDAL().UpdateUser(_user);
        }

        public void DeleteUser(int _id)
        {
            new AdminDAL().DeleteUser(_id);
        }
        #endregion

        #region Company_SaleRep
        public List<Company_SaleRep> getCompany_SaleRepList()
        {
            return new AdminDAL().getCompany_SaleRepsList();
        }

        public Company_SaleRep getCompany_SaleRepById(int _id)
        {
            return new AdminDAL().getCompany_SaleRepById(_id);
        }

        public bool AddCompany_SaleRep(Company_SaleRep _Company_SaleRep)
        {
            new AdminDAL().AddCompany_SaleRep(_Company_SaleRep);
            return true;
        }

        public bool UpdateCompany_SaleRep(Company_SaleRep _Company_SaleRep)
        {
            new AdminDAL().UpdateCompany_SaleRep(_Company_SaleRep);
            return true;
        }

        public void DeleteCompany_SaleRep(int _id)
        {
            new AdminDAL().DeleteCompany_SaleRep(_id);
        }
        #endregion

        #region Company
        public List<Company> getCompanyList()
        {
            return new AdminDAL().getCompanysList();
        }

        public Company getCompanyById(int _id)
        {
            return new AdminDAL().getCompanyById(_id);
        }

        public bool AddCompany(Company _Company)
        {
            if (_Company.Name == null || _Company.Email == null  || _Company.Website == null || _Company.Phone == null )

                return false;
            return new AdminDAL().AddCompany(_Company);
        }

        public bool UpdateCompany(Company _Company)
        {
            if (_Company.Name == null || _Company.Email == null  || _Company.Website == null || _Company.Phone == null)

                return false;

            return new AdminDAL().UpdateCompany(_Company);
        }

        public void DeleteCompany(int _id)
        {
            new AdminDAL().DeleteCompany(_id);
        }
        #endregion

        #region Visitor
        public List<Visitor> getVisitorList()
        {
            return new AdminDAL().getVisitorsList();
        }

        public Visitor getVisitorById(int _id)
        {
            return new AdminDAL().getVisitorById(_id);
        }

        public bool AddVisitor(Visitor _Visitor)
        {
            if (_Visitor.Name == null || _Visitor.Email == null || _Visitor.BusinessName == null || _Visitor.Address == null || _Visitor.ZipCode == null || _Visitor.City == null || _Visitor.Phone == null || _Visitor.BestMethod == null || _Visitor.BestTime == null)

                return false;
            return new AdminDAL().AddVisitor(_Visitor);
        }

        public bool UpdateVisitor(Visitor _Visitor)
        {
            if (_Visitor.Name == null || _Visitor.Email == null || _Visitor.BusinessName == null || _Visitor.Address == null || _Visitor.ZipCode == null || _Visitor.City == null || _Visitor.Phone == null || _Visitor.BestMethod == null || _Visitor.BestTime == null)

                return false;

            return new AdminDAL().UpdateVisitor(_Visitor);
        }

        public void DeleteVisitor(int _id)
        {
            new AdminDAL().DeleteVisitor(_id);
        }
        #endregion

        #region SaleRepresentative
        public List<SaleRepresentative> getSaleRepresentativeList()
        {
            return new AdminDAL().getSaleRepresentativesList();
        }

        public SaleRepresentative getSaleRepresentativeById(int _id)
        {
            return new AdminDAL().getSaleRepresentativeById(_id);
        }

        public bool AddSaleRepresentative(SaleRepresentative _SaleRepresentative)
        {
            if (_SaleRepresentative.Name == null || _SaleRepresentative.Email == null || _SaleRepresentative.SalesFirmName == null ||  _SaleRepresentative.EmailReceiveLeads == null)

                return false;
            return new AdminDAL().AddSaleRepresentative(_SaleRepresentative);
        }

        public bool UpdateSaleRepresentative(SaleRepresentative _SaleRepresentative)
        {
            if (_SaleRepresentative.Name == null || _SaleRepresentative.Email == null || _SaleRepresentative.SalesFirmName == null ||  _SaleRepresentative.EmailReceiveLeads == null)

                return false;

            return new AdminDAL().UpdateSaleRepresentative(_SaleRepresentative);
        }

        public void DeleteSaleRepresentative(int _id)
        {
            new AdminDAL().DeleteSaleRepresentative(_id);
        }
        #endregion

        #region States
        public List<State> getStateList()
        {
            return new AdminDAL().getStateList();
        }

        public State getStateById(int _id)
        {
            return new AdminDAL().getStateById(_id);
        }

        public bool AddState(State _SaleRepresentative)
        {
            if (_SaleRepresentative.Name == null)

                return false;
            return new AdminDAL().AddState(_SaleRepresentative);
        }

        public bool UpdateState(State _SaleRepresentative)
        {
            if (_SaleRepresentative.Name == null)
                return false;

            return new AdminDAL().UpdateState(_SaleRepresentative);
        }

        public void DeleteState(int _id)
        {
            new AdminDAL().DeleteState(_id);
        }
        #endregion

        #region Visitor_States
        public List<Visitor_States> getVisitor_StatesList()
        {
            return new AdminDAL().getVisitor_StatesList();
        }

        public Visitor_States getVisitor_StatesById(int _id)
        {
            return new AdminDAL().getVisitor_StatesById(_id);
        }

        public bool AddVisitor_States(Visitor_States _SaleRepresentative)
        {
            if (_SaleRepresentative.VisitorId == null || _SaleRepresentative.StateId == null)

                return false;
            return new AdminDAL().AddVisitor_States(_SaleRepresentative);
        }

        public bool UpdateVisitor_States(Visitor_States _SaleRepresentative)
        {
            if (_SaleRepresentative.VisitorId == null || _SaleRepresentative.StateId == null)
                return false;

            return new AdminDAL().UpdateVisitor_States(_SaleRepresentative);
        }

        public void DeleteVisitor_States(int _id)
        {
            new AdminDAL().DeleteVisitor_States(_id);
        }
        #endregion

        #region RepStatePart
        public List<RepStatePart> getRepStatePartList()
        {
            return new AdminDAL().getRepStatePartList();
        }

        public RepStatePart getRepStatePartById(int _id)
        {
            return new AdminDAL().getRepStatePartById(_id);
        }

        public bool AddRepStatePart(RepStatePart _SaleRepresentative)
        {
            if (_SaleRepresentative.StateId == null || _SaleRepresentative.SaleRepId == null)

                return false;
            return new AdminDAL().AddRepStatePart(_SaleRepresentative);
        }

        public bool UpdateRepStatePart(RepStatePart _SaleRepresentative)
        {
            if (_SaleRepresentative.StateId == null || _SaleRepresentative.SaleRepId == null)

                return false;
            return new AdminDAL().UpdateRepStatePart(_SaleRepresentative);
        }

        public void DeleteRepStatePart(int _id)
        {
            new AdminDAL().DeleteRepStatePart(_id);
        }
        #endregion

        #region Campaign
        public List<Campaign> getCampaignList()
        {
            return new AdminDAL().getCampaignList();
        }

        public Campaign getCampaignById(int _id)
        {
            return new AdminDAL().getCampaignById(_id);
        }

        public bool AddCampaign(Campaign _SaleRepresentative)
        {
            if (_SaleRepresentative.Name == null || _SaleRepresentative.Url == null)

                return false;
            return new AdminDAL().AddCampaign(_SaleRepresentative);
        }

        public bool UpdateCampaign(Campaign _SaleRepresentative)
        {
            if (_SaleRepresentative.Name == null || _SaleRepresentative.Url == null)

                return false;
            return new AdminDAL().UpdateCampaign(_SaleRepresentative);
        }

        public void DeleteCampaign(int _id)
        {
            new AdminDAL().DeleteCampaign(_id);
        }
        #endregion
    }
}